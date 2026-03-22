using System;
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
                throw new ArgumentException("Value must be finite.", nameof(value));

            if (!Enum.IsDefined(typeof(TUnit), unit))
                throw new ArgumentException("Invalid unit.", nameof(unit));

            Value = value;
            Unit = unit;
        }

        public Quantity<TUnit> ConvertTo(TUnit targetUnit)
        {
            ValidateTargetUnit(targetUnit);

            double baseValue = ToBaseUnit();
            double converted = FromBaseUnit(baseValue, targetUnit);

            return new Quantity<TUnit>(converted, targetUnit);
        }

        public Quantity<TUnit> Add(Quantity<TUnit> other)
        {
            return Add(other, Unit);
        }

        public Quantity<TUnit> Add(Quantity<TUnit> other, TUnit targetUnit)
        {
            ValidateArithmeticOperands(other, targetUnit, true);

            double baseResult = PerformBaseArithmetic(other, ArithmeticOperation.Add);
            double converted = FromBaseUnit(baseResult, targetUnit);

            return new Quantity<TUnit>(RoundToTwoDecimals(converted), targetUnit);
        }

        public Quantity<TUnit> Subtract(Quantity<TUnit> other)
        {
            return Subtract(other, Unit);
        }

        public Quantity<TUnit> Subtract(Quantity<TUnit> other, TUnit targetUnit)
        {
            ValidateArithmeticOperands(other, targetUnit, true);

            double baseResult = PerformBaseArithmetic(other, ArithmeticOperation.Subtract);
            double converted = FromBaseUnit(baseResult, targetUnit);

            return new Quantity<TUnit>(RoundToTwoDecimals(converted), targetUnit);
        }

        public double Divide(Quantity<TUnit> other)
        {
            ValidateArithmeticOperands(other, default, false);

            return PerformBaseArithmetic(other, ArithmeticOperation.Divide);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Quantity<TUnit> other)
                return false;

            return Math.Abs(ToBaseUnit() - other.ToBaseUnit()) < Epsilon;
        }

        public override int GetHashCode()
        {
            return ToBaseUnit().GetHashCode();
        }

        public override string ToString()
        {
            return $"{Value} {Unit}";
        }

        private void ValidateArithmeticOperands(
            Quantity<TUnit>? other,
            TUnit targetUnit,
            bool targetUnitRequired)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other), "Other quantity cannot be null.");

            if (!double.IsFinite(Value))
                throw new ArgumentException("Current quantity value must be finite.");

            if (!double.IsFinite(other.Value))
                throw new ArgumentException("Other quantity value must be finite.");

            if (targetUnitRequired)
                ValidateTargetUnit(targetUnit);
        }

        private void ValidateTargetUnit(TUnit targetUnit)
        {
            if (!Enum.IsDefined(typeof(TUnit), targetUnit))
                throw new ArgumentException("Invalid target unit.", nameof(targetUnit));
        }

        private double PerformBaseArithmetic(
            Quantity<TUnit> other,
            ArithmeticOperation operation)
        {
            double thisBase = ToBaseUnit();
            double otherBase = other.ToBaseUnit();

            return operation switch
            {
                ArithmeticOperation.Add => thisBase + otherBase,
                ArithmeticOperation.Subtract => thisBase - otherBase,
                ArithmeticOperation.Divide => Math.Abs(otherBase) < Epsilon
                    ? throw new ArithmeticException("Cannot divide by zero.")
                    : thisBase / otherBase,
                _ => throw new InvalidOperationException("Unsupported arithmetic operation.")
            };
        }

        private double ToBaseUnit()
        {
            if (typeof(TUnit) == typeof(LengthUnit))
            {
                return UnitConversionService.ConvertLengthToBase(
                    Value, (LengthUnit)(object)Unit);
            }

            if (typeof(TUnit) == typeof(WeightUnit))
            {
                return UnitConversionService.ConvertWeightToBase(
                    Value, (WeightUnit)(object)Unit);
            }

            if (typeof(TUnit) == typeof(VolumeUnit))
            {
                var unit = (VolumeUnit)(object)Unit;

                return unit switch
                {
                    VolumeUnit.Litre => Value,
                    VolumeUnit.Millilitre => Value * 0.001,
                    VolumeUnit.Gallon => Value * 3.78541,
                    _ => throw new ArgumentException("Invalid VolumeUnit.")
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
                    _ => throw new ArgumentException("Invalid LengthUnit.")
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
                    _ => throw new ArgumentException("Invalid WeightUnit.")
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
                    _ => throw new ArgumentException("Invalid VolumeUnit.")
                };
            }

            throw new InvalidOperationException("Unsupported unit type.");
        }

        private static double RoundToTwoDecimals(double value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        private enum ArithmeticOperation
        {
            Add,
            Subtract,
            Divide
        }
    }
}