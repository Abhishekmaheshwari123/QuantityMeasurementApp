using System;
using QuantityMeasurement.Domain.ValueObjects;

namespace QuantityMeasurement.Domain.Services
{
    /// <summary>
    /// Service for comparing quantity measurements.
    /// UC2 focuses on comparing different quantity measurements for equality.
    /// This service abstracts the comparison logic, allowing for future extensions (e.g., adding more units).
    /// </summary>
    public sealed class QuantityComparisonService
    {
        /// <summary>
        /// Compares two quantity measurements for equality.
        /// </summary>
        /// <param name="first">The first quantity measurement.</param>
        /// <param name="second">The second quantity measurement.</param>
        /// <returns>True if both measurements are equal; otherwise false.</returns>
        public bool AreEqual(QuantityLength first, QuantityLength second)
        {
            if (first is null)
                throw new ArgumentNullException(nameof(first));

            if (second is null)
                throw new ArgumentNullException(nameof(second));

            // Use the value-based equality defined in QuantityLength, which handles unit conversion internally.
            return first.Equals(second);
        }
    }
}