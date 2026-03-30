using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurement.Api.Contracts
{
    /// <summary>
    /// Input payload for user registration.
    /// </summary>
    public sealed class SignupRequest
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

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
