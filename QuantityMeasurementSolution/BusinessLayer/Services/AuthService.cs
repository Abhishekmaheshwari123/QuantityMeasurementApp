using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using BusinessLayer.Interfaces;
using ModelLayer.DTOs;
using QuantityMeasurement.ModelLayer.Entities;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    /// <summary>
    /// Handles sign-up and credential verification for API users.
    /// </summary>
    public sealed class AuthService : IAuthService
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100_000;

        private readonly IUserAuthRepository _userAuthRepository;

        public AuthService(IUserAuthRepository userAuthRepository)
        {
            _userAuthRepository = userAuthRepository ?? throw new ArgumentNullException(nameof(userAuthRepository));
        }

        public async Task<SignupResultDto> SignUpAsync(
            string fullName,
            string email,
            string password,
            CancellationToken cancellationToken = default)
        {
            // Normalize inputs once so validation and repository checks are consistent.
            string normalizedName = fullName?.Trim() ?? string.Empty;
            string normalizedEmail = NormalizeEmail(email);

            if (string.IsNullOrWhiteSpace(normalizedName))
                return new SignupResultDto(false, "Full name is required.");

            if (string.IsNullOrWhiteSpace(normalizedEmail))
                return new SignupResultDto(false, "Email is required.");

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return new SignupResultDto(false, "Password must be at least 8 characters long.");

            if (await _userAuthRepository.EmailExistsAsync(normalizedEmail, cancellationToken))
                return new SignupResultDto(false, "Email is already registered.");

            var user = new UserRecord
            {
                FullName = normalizedName,
                Email = normalizedEmail,
                // Password is never persisted as plaintext.
                PasswordHash = HashPassword(password)
            };

            await _userAuthRepository.AddAsync(user, cancellationToken);

            return new SignupResultDto(true, "User registered successfully.");
        }

        public async Task<AuthenticatedUserDto?> LoginAsync(
            string email,
            string password,
            CancellationToken cancellationToken = default)
        {
            string normalizedEmail = NormalizeEmail(email);

            if (string.IsNullOrWhiteSpace(normalizedEmail) || string.IsNullOrWhiteSpace(password))
                return null;

            UserRecord? user = await _userAuthRepository.GetByEmailAsync(normalizedEmail, cancellationToken);
            if (user == null)
                return null;

            // Return null for invalid credentials to avoid exposing which part failed.
            if (!VerifyPassword(password, user.PasswordHash))
                return null;

            return new AuthenticatedUserDto(user.Id, user.FullName, user.Email);
        }

        private static string NormalizeEmail(string email)
        {
            return (email ?? string.Empty).Trim().ToLowerInvariant();
        }

        private static string HashPassword(string password)
        {
            // PBKDF2 with per-user random salt slows down brute-force attacks.
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                KeySize);

            // Format: iterations.base64salt.base64hash
            return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        private static bool VerifyPassword(string password, string storedHash)
        {
            // Gracefully reject malformed persisted values.
            string[] parts = (storedHash ?? string.Empty).Split('.');
            if (parts.Length != 3)
                return false;

            if (!int.TryParse(parts[0], out int iterations))
                return false;

            byte[] salt;
            byte[] expectedHash;

            try
            {
                salt = Convert.FromBase64String(parts[1]);
                expectedHash = Convert.FromBase64String(parts[2]);
            }
            catch (FormatException)
            {
                return false;
            }

            byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                expectedHash.Length);

            // Constant-time comparison reduces timing attack signal.
            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
    }
}
