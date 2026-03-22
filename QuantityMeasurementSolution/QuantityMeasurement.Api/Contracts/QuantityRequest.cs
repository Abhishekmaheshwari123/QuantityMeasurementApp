using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurement.Api.Contracts
{
    public sealed class QuantityRequest
    {
        [Required]
        public double Value { get; set; }

        [Required]
        [MinLength(1)]
        public string Unit { get; set; } = string.Empty;

        [Required]
        [MinLength(1)]
        public string MeasurementType { get; set; } = string.Empty;
    }
}