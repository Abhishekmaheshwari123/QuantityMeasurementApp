using System.Threading;
using System.Threading.Tasks;
using ModelLayer.DTOs;

namespace BusinessLayer.Interfaces
{
    /// <summary>
    /// Defines business operations for user registration and authentication.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new user account when the email is not already in use.
        /// </summary>
        Task<SignupResultDto> SignUpAsync(string fullName, string email, string password, CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates credentials and returns authenticated user details when valid.
        /// </summary>
        Task<AuthenticatedUserDto?> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
    }
}
