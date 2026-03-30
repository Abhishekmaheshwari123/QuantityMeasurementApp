namespace ModelLayer.DTOs
{
    /// <summary>
    /// Represents the outcome of a signup attempt with a user-facing message.
    /// </summary>
    public sealed record SignupResultDto(bool Success, string Message);
}
