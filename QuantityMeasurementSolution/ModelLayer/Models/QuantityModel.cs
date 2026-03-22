using System;

namespace ModelLayer.Models
{
    public sealed class QuantityModel<TUnit> where TUnit : struct, Enum
    {
        public double Value { get; }
        public TUnit Unit { get; }

        public QuantityModel(double value, TUnit unit)
        {
            Value = value;
            Unit = unit;
        }

        public override string ToString()
        {
            return $"{Value} {Unit}";
        }
    }
}