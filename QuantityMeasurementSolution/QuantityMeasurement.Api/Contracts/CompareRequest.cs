using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurement.Api.Contracts
{
    public sealed class CompareRequest
    {
        [Required]
        public QuantityRequest FirstQuantity { get; set; } = new();

        [Required]
        public QuantityRequest SecondQuantity { get; set; } = new();
    }
}