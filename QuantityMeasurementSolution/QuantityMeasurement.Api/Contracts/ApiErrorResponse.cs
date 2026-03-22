namespace QuantityMeasurement.Api.Contracts
{
    public sealed class ApiErrorResponse
    {
        public DateTime TimestampUtc { get; init; } = DateTime.UtcNow;
        public int Status { get; init; }
        public string Error { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public string Path { get; init; } = string.Empty;
    }
}
