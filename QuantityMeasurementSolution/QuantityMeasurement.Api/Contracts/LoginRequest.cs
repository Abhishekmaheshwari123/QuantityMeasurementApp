using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurement.Api.Contracts
{
    /// <summary>
    /// Input payload for user login.
    /// </summary>
    public sealed class LoginRequest
    {
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        [MaxLength(100)]
        public string Password { get; set; } = string.Empty;
    }
}
