namespace ModelLayer.DTOs
{
    /// <summary>
    /// Lightweight authenticated user shape returned by login flow.
    /// </summary>
    public sealed record AuthenticatedUserDto(long Id, string FullName, string Email);
}
