using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuantityMeasurement.Api.Contracts;

namespace QuantityMeasurement.Api.Controllers
{
    /// <summary>
    /// Provides anonymous endpoints for user signup and login.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public sealed class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        [HttpPost("signup")]
        public async Task<ActionResult<ApiResponse<string>>> Signup([FromBody] SignupRequest request, CancellationToken cancellationToken)
        {
            // Delegate validation + persistence to business layer.
            var signupResult = await _authService.SignUpAsync(
                request.FullName,
                request.Email,
                request.Password,
                cancellationToken);

            if (!signupResult.Success)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = signupResult.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = signupResult.Message,
                Data = null
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            // Service returns null when credentials are invalid.
            var user = await _authService.LoginAsync(request.Email, request.Password, cancellationToken);
            if (user == null)
            {
                return Unauthorized(new ApiResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Invalid email or password.",
                    Data = null
                });
            }

            AuthResponse authResponse = GenerateJwt(user.Email, user.FullName, user.Id);

            return Ok(new ApiResponse<AuthResponse>
            {
                Success = true,
                Message = "Login successful.",
                Data = authResponse
            });
        }

        private AuthResponse GenerateJwt(string email, string fullName, long userId)
        {
            string jwtKey = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT key is missing from configuration.");

            string issuer = _configuration["Jwt:Issuer"] ?? "QuantityMeasurement.Api";
            string audience = _configuration["Jwt:Audience"] ?? "QuantityMeasurement.Client";
            int expiryMinutes = int.TryParse(_configuration["Jwt:ExpiryMinutes"], out int configuredMinutes)
                ? configuredMinutes
                : 60;

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                // Subject and email are standard JWT claims.
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Email, email),
                // Custom claims used by the API when needed.
                new Claim("name", fullName),
                new Claim("userId", userId.ToString())
            };

            DateTime expiresAtUtc = DateTime.UtcNow.AddMinutes(expiryMinutes);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAtUtc,
                signingCredentials: credentials);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return new AuthResponse(tokenString, expiresAtUtc, email, fullName);
        }
    }
}
