using System;

namespace ModelLayer.DTOs
{
    public sealed class QuantityDto
    {
        public double Value { get; }
        public string Unit { get; }
        public string MeasurementType { get; }

        public QuantityDto(double value, string unit, string measurementType)
        {
            if (!double.IsFinite(value))
                throw new ArgumentException("Value must be finite.", nameof(value));

            if (string.IsNullOrWhiteSpace(unit))
                throw new ArgumentException("Unit is required.", nameof(unit));

            if (string.IsNullOrWhiteSpace(measurementType))
                throw new ArgumentException("Measurement type is required.", nameof(measurementType));

            Value = value;
            Unit = unit.Trim();
            MeasurementType = measurementType.Trim();
        }

        public override string ToString()
        {
            return $"{Value} {Unit}";
        }
    }
}