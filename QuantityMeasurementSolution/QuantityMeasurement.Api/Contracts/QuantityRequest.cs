namespace QuantityMeasurement.Api.Contracts
{
    public sealed class QuantityRequest
    {
        public double Value { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string MeasurementType { get; set; } = string.Empty;
    }
}