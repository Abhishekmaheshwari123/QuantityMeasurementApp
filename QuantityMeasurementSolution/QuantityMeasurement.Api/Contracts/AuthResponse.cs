namespace QuantityMeasurement.Api.Contracts
{
    /// <summary>
    /// API payload returned after successful login.
    /// </summary>
    public sealed record AuthResponse(string Token, DateTime ExpiresAtUtc, string Email, string FullName);
}
