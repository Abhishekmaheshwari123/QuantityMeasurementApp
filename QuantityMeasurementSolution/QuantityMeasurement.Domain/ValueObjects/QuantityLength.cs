using System;
using QuantityMeasurement.Domain.Enums;

namespace QuantityMeasurement.Domain.ValueObjects
{
    /// <summary>
    /// Represents a generic length measurement with value and unit.
    /// Focuses on equality and arithmetic while delegating all conversion
    /// responsibility to <see cref="LengthUnit"/> (UC8).
    /// </summary>
    public sealed class QuantityLength : IEquatable<QuantityLength>
    {
        /// <summary>
        /// Gets the numeric value of the quantity.
        /// </summary>
        public double Value { get; }

        /// <summary>
        /// Gets the unit type of the quantity.
        /// </summary>
        public LengthUnit Unit { get; }

        /// <summary>
        /// Epsilon used for floating-point comparisons.
        /// </summary>
        private const double Epsilon = 1e-6;

        /// <summary>
        /// Initializes a new instance of <see cref="QuantityLength"/>.
        /// </summary>
        /// <param name="value">Numeric value.</param>
        /// <param name="unit">Length unit.</param>
        /// <exception cref="ArgumentException">Thrown when value is NaN/Infinity.</exception>
        public QuantityLength(double value, LengthUnit unit)
        {
            if (!double.IsFinite(value))
                throw new ArgumentException("Value must be a finite number.", nameof(value));

            // LengthUnit is an enum, so it can't be null, but invalid cast values can exist.
            if (!Enum.IsDefined(typeof(LengthUnit), unit))
                throw new ArgumentException("Invalid unit.", nameof(unit));

            Value = value;
            Unit = unit;
        }

        /// <summary>
        /// Converts current value to base unit (Feet) using the unit conversion responsibility.
        /// </summary>
        /// <returns>Value converted to base unit (Feet).</returns>
        private double ToBaseFeet()
        {
            return Unit.ConvertToBaseUnit(Value);
        }

        /// <summary>
        /// Checks value-based equality using base-unit comparison.
        /// </summary>
        /// <param name="other">Other quantity.</param>
        /// <returns>True if both are equal within epsilon; otherwise false.</returns>
        public bool Equals(QuantityLength? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            double baseThis = Unit.ConvertToBaseUnit(Value);
            double baseOther = other.Unit.ConvertToBaseUnit(other.Value);

            return Math.Abs(baseThis - baseOther) < Epsilon;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is QuantityLength other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            // Hash based on base-unit value to keep consistent across units.
            return ToBaseFeet().GetHashCode();
        }

        public static bool operator ==(QuantityLength? left, QuantityLength? right) => Equals(left, right);
        public static bool operator !=(QuantityLength? left, QuantityLength? right) => !Equals(left, right);

        /// <inheritdoc/>
        public override string ToString() => $"{Value} {Unit}";

        /// <summary>
        /// Converts a numeric value between units using UC8 unit responsibility:
        /// source converts to base, target converts from base.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="source">Source unit.</param>
        /// <param name="target">Target unit.</param>
        /// <returns>Converted value in target unit.</returns>
        /// <exception cref="ArgumentException">Thrown when value is NaN/Infinity or units are invalid.</exception>
        public static double Convert(double value, LengthUnit source, LengthUnit target)
        {
            if (!double.IsFinite(value))
                throw new ArgumentException("Value must be finite.", nameof(value));

            if (!Enum.IsDefined(typeof(LengthUnit), source))
                throw new ArgumentException("Invalid source unit.", nameof(source));

            if (!Enum.IsDefined(typeof(LengthUnit), target))
                throw new ArgumentException("Invalid target unit.", nameof(target));

            if (source == target)
                return value;

            // UC8: delegate conversions to unit methods.
            double baseFeet = source.ConvertToBaseUnit(value);
            return target.ConvertFromBaseUnit(baseFeet);
        }

        /// <summary>
        /// Converts this quantity to the specified target unit.
        /// </summary>
        /// <param name="target">Target unit.</param>
        /// <returns>New immutable quantity in target unit.</returns>
        public QuantityLength ConvertTo(LengthUnit target)
        {
            double convertedValue = Convert(Value, Unit, target);
            return new QuantityLength(convertedValue, target);
        }

        /// <summary>
        /// Adds another quantity to the current instance.
        /// Result is returned in the unit of the first operand (UC6).
        /// </summary>
        /// <param name="other">The second quantity.</param>
        /// <returns>New quantity in the first operand's unit.</returns>
        public QuantityLength Add(QuantityLength other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other), "Second quantity cannot be null.");

            // Convert both to base unit (Feet)
            double baseThis = Unit.ConvertToBaseUnit(Value);
            double baseOther = other.Unit.ConvertToBaseUnit(other.Value);

            double baseSum = baseThis + baseOther;

            // Convert back to first operand unit
            double resultValue = Unit.ConvertFromBaseUnit(baseSum);

            return new QuantityLength(resultValue, Unit);
        }

        /// <summary>
        /// Static overload: adds two quantities, returning result in the unit of the first operand.
        /// </summary>
        public static QuantityLength Add(QuantityLength first, QuantityLength second)
        {
            if (first is null)
                throw new ArgumentNullException(nameof(first));

            return first.Add(second);
        }

        /// <summary>
        /// Adds two quantities and returns result in an explicitly specified target unit (UC7).
        /// </summary>
        /// <param name="first">First operand.</param>
        /// <param name="second">Second operand.</param>
        /// <param name="targetUnit">Explicit result unit.</param>
        /// <returns>New quantity in target unit.</returns>
        public static QuantityLength Add(QuantityLength first, QuantityLength second, LengthUnit targetUnit)
        {
            if (first is null)
                throw new ArgumentNullException(nameof(first));

            if (second is null)
                throw new ArgumentNullException(nameof(second));

            if (!Enum.IsDefined(typeof(LengthUnit), targetUnit))
                throw new ArgumentException("Invalid target unit.", nameof(targetUnit));

            double baseFirst = first.Unit.ConvertToBaseUnit(first.Value);
            double baseSecond = second.Unit.ConvertToBaseUnit(second.Value);

            double baseSum = baseFirst + baseSecond;

            double finalValue = targetUnit.ConvertFromBaseUnit(baseSum);

            return new QuantityLength(finalValue, targetUnit);
        }
    }
}