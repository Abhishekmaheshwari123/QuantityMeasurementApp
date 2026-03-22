using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurement.Api.Contracts
{
    public sealed class ConvertRequest
    {
        [Required]
        public QuantityRequest SourceQuantity { get; set; } = new();

        [Required]
        [MinLength(1)]
        public string TargetUnit { get; set; } = string.Empty;
    }
}