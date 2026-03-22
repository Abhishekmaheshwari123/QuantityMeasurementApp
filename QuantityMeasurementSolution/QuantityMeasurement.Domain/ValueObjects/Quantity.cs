using QuantityMeasurement.Domain.Enums;
using QuantityMeasurement.Domain.Services;

namespace QuantityMeasurement.Domain.ValueObjects
{
    public sealed class Quantity<TUnit> where TUnit : struct, Enum
    {
        private const double Epsilon = 1e-6;

        public double Value { get; }
        public TUnit Unit { get; }

        public Quantity(double value, TUnit unit)
        {
            if (!double.IsFinite(value))
                throw new ArgumentException("Value must be finite.");

            Value = value;
            Unit = unit;
        }

        private double ToBaseUnit()
        {
            if (typeof(TUnit) == typeof(LengthUnit))
                return UnitConversionService.ConvertLengthToBase(
                    Value, (LengthUnit)(object)Unit);

            if (typeof(TUnit) == typeof(WeightUnit))
                return UnitConversionService.ConvertWeightToBase(
                    Value, (WeightUnit)(object)Unit);

            if (typeof(TUnit) == typeof(VolumeUnit))
            {
                var unit = (VolumeUnit)(object)Unit;

                return unit switch
                {
                    VolumeUnit.Litre => Value,
                    VolumeUnit.Millilitre => Value * 0.001,
                    VolumeUnit.Gallon => Value * 3.78541,
                    _ => throw new ArgumentException("Invalid VolumeUnit")
                };
            }

            throw new InvalidOperationException("Unsupported unit type.");
        }

        private static double FromBaseUnit(double baseValue, TUnit targetUnit)
        {
            if (typeof(TUnit) == typeof(LengthUnit))
            {
                var unit = (LengthUnit)(object)targetUnit;

                return unit switch
                {
                    LengthUnit.Feet => baseValue,
                    LengthUnit.Inches => baseValue * 12.0,
                    LengthUnit.Yards => baseValue / 3.0,
                    LengthUnit.Centimeters => baseValue / 0.0328084,
                    _ => throw new ArgumentException("Invalid LengthUnit")
                };
            }

            if (typeof(TUnit) == typeof(WeightUnit))
            {
                var unit = (WeightUnit)(object)targetUnit;

                return unit switch
                {
                    WeightUnit.Gram => baseValue,
                    WeightUnit.Kilogram => baseValue / 1000.0,
                    WeightUnit.Pound => baseValue / 453.592,
                    WeightUnit.Tonne => baseValue / 1_000_000.0,
                    _ => throw new ArgumentException("Invalid WeightUnit")
                };
            }

            if (typeof(TUnit) == typeof(VolumeUnit))
            {
                var unit = (VolumeUnit)(object)targetUnit;

                return unit switch
                {
                    VolumeUnit.Litre => baseValue,
                    VolumeUnit.Millilitre => baseValue / 0.001,
                    VolumeUnit.Gallon => baseValue / 3.78541,
                    _ => throw new ArgumentException("Invalid VolumeUnit")
                };
            }

            throw new InvalidOperationException("Unsupported unit type.");
        }

        private static double Round2(double value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        public Quantity<TUnit> Add(Quantity<TUnit> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            double baseSum = ToBaseUnit() + other.ToBaseUnit();
            double result = FromBaseUnit(baseSum, Unit);

            return new Quantity<TUnit>(result, Unit);
        }

        public Quantity<TUnit> Add(Quantity<TUnit> other, TUnit targetUnit)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            double baseSum = ToBaseUnit() + other.ToBaseUnit();
            double result = FromBaseUnit(baseSum, targetUnit);

            return new Quantity<TUnit>(result, targetUnit);
        }

        public Quantity<TUnit> Subtract(Quantity<TUnit> other)
            => Subtract(other, Unit);

        public Quantity<TUnit> Subtract(Quantity<TUnit> other, TUnit targetUnit)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (!double.IsFinite(other.Value))
                throw new ArgumentException("Other value must be finite.", nameof(other));

            double diffBase = ToBaseUnit() - other.ToBaseUnit();
            double converted = FromBaseUnit(diffBase, targetUnit);
            double rounded = Round2(converted);

            return new Quantity<TUnit>(rounded, targetUnit);
        }

        public double Divide(Quantity<TUnit> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (!double.IsFinite(other.Value))
                throw new ArgumentException("Other value must be finite.", nameof(other));

            double denominatorBase = other.ToBaseUnit();

            if (Math.Abs(denominatorBase) < Epsilon)
                throw new DivideByZeroException("Cannot divide by a zero quantity.");

            double numeratorBase = ToBaseUnit();
            return numeratorBase / denominatorBase;
        }

        public Quantity<TUnit> ConvertTo(TUnit targetUnit)
        {
            double baseValue = ToBaseUnit();
            double result = FromBaseUnit(baseValue, targetUnit);

            return new Quantity<TUnit>(result, targetUnit);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Quantity<TUnit> other)
                return false;

            return Math.Abs(ToBaseUnit() - other.ToBaseUnit()) < Epsilon;
        }

        public override int GetHashCode()
            => ToBaseUnit().GetHashCode();

        public override string ToString()
            => $"{Value} {Unit}";
    }
}