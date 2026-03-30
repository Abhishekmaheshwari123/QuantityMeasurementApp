using System;

namespace QuantityMeasurement.ModelLayer.Entities
{
    /// <summary>
    /// ORM record mapped to dbo.AppUsers for local API authentication.
    /// </summary>
    public sealed class UserRecord
    {
        // Surrogate key used for internal joins and references.
        public long Id { get; set; }

        // Display name chosen by the user during signup.
        public string FullName { get; set; } = string.Empty;

        // Stored in normalized lowercase format for stable uniqueness checks.
        public string Email { get; set; } = string.Empty;

        // Stores PBKDF2 metadata + salt + hash in a single value.
        public string PasswordHash { get; set; } = string.Empty;

        // Audit timestamp in UTC assigned by database default.
        public DateTime CreatedAtUtc { get; set; }
    }
}
