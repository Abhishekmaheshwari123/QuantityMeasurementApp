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

        public Quantity<TUnit> Add(Quantity<TUnit> other)
        {
            double baseSum = ToBaseUnit() + other.ToBaseUnit();

            if (typeof(TUnit) == typeof(LengthUnit))
            {
                var unit = (LengthUnit)(object)Unit;
                double result = baseSum; // already base = Feet
                return new Quantity<TUnit>(result, Unit);
            }

            if (typeof(TUnit) == typeof(WeightUnit))
            {
                var unit = (WeightUnit)(object)Unit;
                double result = baseSum; // already base = Gram
                return new Quantity<TUnit>(result, Unit);
            }

            if (typeof(TUnit) == typeof(VolumeUnit))
           {
               var unit = (VolumeUnit)(object)Unit;
           
               double result = unit switch
               {
                   VolumeUnit.Litre => baseSum,
                   VolumeUnit.Millilitre => baseSum / 0.001,
                   VolumeUnit.Gallon => baseSum / 3.78541,
                   _ => throw new ArgumentException("Invalid VolumeUnit")
               };
           
               return new Quantity<TUnit>(result, Unit);
           }

            throw new InvalidOperationException();
        }

        public Quantity<TUnit> Add(Quantity<TUnit> other, TUnit targetUnit)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            double baseSum = ToBaseUnit() + other.ToBaseUnit();

            // Convert baseSum back to target unit
            if (typeof(TUnit) == typeof(LengthUnit))
            {
                var unit = (LengthUnit)(object)targetUnit;

                double result = unit switch
                {
                    LengthUnit.Feet => baseSum,
                    LengthUnit.Inches => baseSum * 12.0,
                    LengthUnit.Yards => baseSum / 3.0,
                    LengthUnit.Centimeters => baseSum / 0.0328084,
                    _ => throw new ArgumentException("Invalid LengthUnit")
                };

                return new Quantity<TUnit>(result, targetUnit);
            }

            if (typeof(TUnit) == typeof(WeightUnit))
            {
                var unit = (WeightUnit)(object)targetUnit;

                double result = unit switch
                {
                    WeightUnit.Gram => baseSum,
                    WeightUnit.Kilogram => baseSum / 1000.0,
                    WeightUnit.Pound => baseSum / 453.592,
                    WeightUnit.Tonne => baseSum / 1_000_000.0,
                    _ => throw new ArgumentException("Invalid WeightUnit")
                };

                return new Quantity<TUnit>(result, targetUnit);
            }

            if (typeof(TUnit) == typeof(VolumeUnit))
           {
               var unit = (VolumeUnit)(object)targetUnit;
           
               double result = unit switch
               {
                   VolumeUnit.Litre => baseSum,
                   VolumeUnit.Millilitre => baseSum / 0.001,
                   VolumeUnit.Gallon => baseSum / 3.78541,
                   _ => throw new ArgumentException("Invalid VolumeUnit")
               };
           
               return new Quantity<TUnit>(result, targetUnit);
           }

            throw new InvalidOperationException("Unsupported unit type.");
        }

        public Quantity<TUnit> ConvertTo(TUnit targetUnit)
        {
            double baseValue = ToBaseUnit();

            if (typeof(TUnit) == typeof(LengthUnit))
            {
                var unit = (LengthUnit)(object)targetUnit;

                double result = unit switch
                {
                    LengthUnit.Feet => baseValue,
                    LengthUnit.Inches => baseValue * 12.0,
                    LengthUnit.Yards => baseValue / 3.0,
                    LengthUnit.Centimeters => baseValue / 0.0328084,
                    _ => throw new ArgumentException("Invalid LengthUnit")
                };

                return new Quantity<TUnit>(result, targetUnit);
            }

            if (typeof(TUnit) == typeof(WeightUnit))
            {
                var unit = (WeightUnit)(object)targetUnit;

                double result = unit switch
                {
                    WeightUnit.Gram => baseValue,
                    WeightUnit.Kilogram => baseValue / 1000.0,
                    WeightUnit.Pound => baseValue / 453.592,
                    WeightUnit.Tonne => baseValue / 1_000_000.0,
                    _ => throw new ArgumentException("Invalid WeightUnit")
                };

                return new Quantity<TUnit>(result, targetUnit);
            }

            if (typeof(TUnit) == typeof(VolumeUnit))
           {
               var unit = (VolumeUnit)(object)targetUnit;
           
               double result = unit switch
               {
                   VolumeUnit.Litre => baseValue,
                   VolumeUnit.Millilitre => baseValue / 0.001,
                   VolumeUnit.Gallon => baseValue / 3.78541,
                   _ => throw new ArgumentException("Invalid VolumeUnit")
               };
           
               return new Quantity<TUnit>(result, targetUnit);
           }

            throw new InvalidOperationException("Unsupported unit type.");
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