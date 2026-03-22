using System;
using QuantityMeasurement.Domain.Enums;

namespace QuantityMeasurement.Domain.ValueObjects
{
    /// <summary>
    /// Represents immutable weight quantity.
    /// Supports equality, conversion and addition.
    /// </summary>
    public sealed class QuantityWeight : IEquatable<QuantityWeight>
    {
        private const double Epsilon = 1e-6;

        public double Value { get; }
        public WeightUnit Unit { get; }

        public QuantityWeight(double value, WeightUnit unit)
        {
            if (!double.IsFinite(value))
                throw new ArgumentException("Value must be finite.");

            if (!Enum.IsDefined(typeof(WeightUnit), unit))
                throw new ArgumentException("Invalid weight unit.");

            Value = value;
            Unit = unit;
        }

        private double ConvertToBase()
            => Unit.ConvertToBase(Value);

        public bool Equals(QuantityWeight? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            double baseThis = ConvertToBase();
            double baseOther = other.ConvertToBase();

            return Math.Abs(baseThis - baseOther) < Epsilon;
        }

        public override bool Equals(object? obj)
            => obj is QuantityWeight other && Equals(other);

        public override int GetHashCode()
            => ConvertToBase().GetHashCode();

        public static bool operator ==(QuantityWeight? left, QuantityWeight? right)
            => Equals(left, right);

        public static bool operator !=(QuantityWeight? left, QuantityWeight? right)
            => !Equals(left, right);

        /// <summary>
        /// Converts to target unit.
        /// </summary>
        public QuantityWeight ConvertTo(WeightUnit target)
        {
            if (!Enum.IsDefined(typeof(WeightUnit), target))
                throw new ArgumentException("Invalid target unit.");

            double baseValue = ConvertToBase();
            double converted = target.ConvertFromBase(baseValue);

            return new QuantityWeight(converted, target);
        }

        /// <summary>
        /// Addition (result in first operand unit).
        /// </summary>
        public QuantityWeight Add(QuantityWeight other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            double baseSum = ConvertToBase() + other.ConvertToBase();
            double resultValue = Unit.ConvertFromBase(baseSum);

            return new QuantityWeight(resultValue, Unit);
        }

        /// <summary>
        /// Static addition overload.
        /// </summary>
        public static QuantityWeight Add(
            QuantityWeight first,
            QuantityWeight second)
        {
            if (first is null)
                throw new ArgumentNullException(nameof(first));

            return first.Add(second);
        }

        /// <summary>
        /// Addition with explicit target unit.
        /// </summary>
        public static QuantityWeight Add(
            QuantityWeight first,
            QuantityWeight second,
            WeightUnit targetUnit)
        {
            if (first is null)
                throw new ArgumentNullException(nameof(first));

            if (second is null)
                throw new ArgumentNullException(nameof(second));

            if (!Enum.IsDefined(typeof(WeightUnit), targetUnit))
                throw new ArgumentException("Invalid target unit.");

            double baseSum = first.ConvertToBase() + second.ConvertToBase();
            double result = targetUnit.ConvertFromBase(baseSum);

            return new QuantityWeight(result, targetUnit);
        }

        public override string ToString()
            => $"{Value} {Unit}";
    }
}