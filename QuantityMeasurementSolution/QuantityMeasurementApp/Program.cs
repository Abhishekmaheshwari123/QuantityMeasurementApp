using System;
using QuantityMeasurement.Domain.Enums;
using QuantityMeasurement.Domain.Services;
using QuantityMeasurement.Domain.ValueObjects;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// Console entry point for Quantity Measurement Application.
    /// Supports Length + Weight operations.
    /// </summary>
    internal static class Program
    {
        // Using a service to handle comparison logic, adhering to separation of concerns and allowing for future extensibility.
        private static readonly QuantityComparisonService _service = new();

        // Main method to run the console application.
        private static void Main()
        {
            Console.WriteLine("=== Quantity Measurement Application ===\n");

            Console.WriteLine("1. Length Equality Check");
            Console.WriteLine("2. Length Unit Conversion");
            Console.WriteLine("3. Add Two Lengths");
            Console.WriteLine("4. Add Two Lengths with Target Unit");
            Console.WriteLine("5. Weight Equality");
            Console.WriteLine("6. Weight Conversion");
            Console.WriteLine("7. Weight Addition");
            Console.Write("\nChoose option: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DemonstrateLengthEquality();
                    break;
                case "2":
                    DemonstrateLengthConversion();
                    break;
                case "3":
                    DemonstrateLengthAddition();
                    break;
                case "4":
                    DemonstrateLengthAdditionWithTargetUnit();
                    break;
                case "5":
                    DemonstrateWeightEquality();
                    break;
                case "6":
                    DemonstrateWeightConversion();
                    break;
                case "7":
                    DemonstrateWeightAddition();
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        // -------------------- LENGTH --------------------

        private static void DemonstrateLengthEquality()
        {
            QuantityLength? first = ReadLength("Enter first length quantity");
            if (first is null) return;

            QuantityLength? second = ReadLength("Enter second length quantity");
            if (second is null) return;

            bool result = _service.AreEqual(first, second);

            Console.WriteLine($"\nInput: {first} and {second}");
            Console.WriteLine($"Output: Equal ({result})");
        }

        /// <summary>
        /// Demonstrates conversion from one unit to another.
        /// </summary>

        private static void DemonstrateLengthConversion()
        {
            Console.Write("Enter value to convert: ");
            if (!double.TryParse(Console.ReadLine(), out double value))
            {
                Console.WriteLine("Invalid numeric value.");
                return;
            }

            Console.Write("From unit (Feet/Inches/Yards/Centimeters): ");
            if (!Enum.TryParse(Console.ReadLine(), true, out LengthUnit fromUnit))
            {
                Console.WriteLine("Invalid source unit.");
                return;
            }

            Console.Write("To unit (Feet/Inches/Yards/Centimeters): ");
            if (!Enum.TryParse(Console.ReadLine(), true, out LengthUnit toUnit))
            {
                Console.WriteLine("Invalid target unit.");
                return;
            }

            try
            {
                double result = QuantityLength.Convert(value, fromUnit, toUnit);
                Console.WriteLine($"\nConverted Result: {result}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Conversion failed: {ex.Message}");
            }
        }

        private static void DemonstrateLengthAddition()
        {
            QuantityLength? first = ReadLength("Enter first length quantity");
            if (first is null) return;

            QuantityLength? second = ReadLength("Enter second length quantity");
            if (second is null) return;

            try
            {
                QuantityLength result = first.Add(second);

                Console.WriteLine($"\nResult: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Addition failed: {ex.Message}");
            }
        }

        private static void DemonstrateLengthAdditionWithTargetUnit()
        {
            QuantityLength? first = ReadLength("Enter first length quantity");
            if (first is null) return;

            QuantityLength? second = ReadLength("Enter second length quantity");
            if (second is null) return;

            Console.Write("Enter target unit (Feet/Inches/Yards/Centimeters): ");
            if (!Enum.TryParse(Console.ReadLine(), true, out LengthUnit targetUnit))
            {
                Console.WriteLine("Invalid target unit.");
                return;
            }

            try
            {
                QuantityLength result = QuantityLength.Add(first, second, targetUnit);
                Console.WriteLine($"\nResult: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Addition failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Reads a quantity from user input.
        /// </summary>
        /// <param name="label">Label shown to the user.</param>
        /// <returns>QuantityLength instance if valid; otherwise null.</returns>
        private static QuantityLength? ReadLength(string label)
        {
            Console.WriteLine($"\n{label}");

            Console.Write("Enter numeric value: ");
            if (!double.TryParse(Console.ReadLine(), out double value))
            {
                Console.WriteLine("Invalid numeric value.");
                return null;
            }

            Console.Write("Enter unit (Feet/Inches/Yards/Centimeters): ");
            string? unitInput = Console.ReadLine();

            // Try parsing the unit input to the LengthUnit enum, ignoring case sensitivity.
            if (!Enum.TryParse(unitInput, true, out LengthUnit unit))
            {
                Console.WriteLine("Invalid unit type.");
                return null;
            }

            try
            {
                // Return a new QuantityLength instance with the provided value and unit.
                return new QuantityLength(value, unit);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Invalid input: {ex.Message}");
                return null;
            }
        }

        // -------------------- WEIGHT --------------------

        private static void DemonstrateWeightEquality()
        {
            QuantityWeight? first = ReadWeight("Enter first weight quantity");
            if (first is null) return;

            QuantityWeight? second = ReadWeight("Enter second weight quantity");
            if (second is null) return;

            bool result = first.Equals(second);

            Console.WriteLine($"\nInput: {first} and {second}");
            Console.WriteLine($"Output: Equal ({result})");
        }

        private static void DemonstrateWeightConversion()
        {
            Console.Write("Enter value to convert: ");
            if (!double.TryParse(Console.ReadLine(), out double value))
            {
                Console.WriteLine("Invalid numeric value.");
                return;
            }

            Console.Write("From unit (Kilogram/Gram/Pound): ");
            if (!Enum.TryParse(Console.ReadLine(), true, out WeightUnit fromUnit))
            {
                Console.WriteLine("Invalid source unit.");
                return;
            }

            Console.Write("To unit (Kilogram/Gram/Pound): ");
            if (!Enum.TryParse(Console.ReadLine(), true, out WeightUnit toUnit))
            {
                Console.WriteLine("Invalid target unit.");
                return;
            }

            try
            {
                // Convert by creating quantity then converting.
                var q = new QuantityWeight(value, fromUnit);
                var converted = q.ConvertTo(toUnit);

                Console.WriteLine($"\nConverted Result: {converted.Value} {converted.Unit}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Conversion failed: {ex.Message}");
            }
        }

        private static void DemonstrateWeightAddition()
        {
            QuantityWeight? first = ReadWeight("Enter first weight quantity");
            if (first is null) return;

            QuantityWeight? second = ReadWeight("Enter second weight quantity");
            if (second is null) return;

            try
            {
                QuantityWeight result = first.Add(second);
                Console.WriteLine($"\nResult: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Addition failed: {ex.Message}");
            }
        }

        private static QuantityWeight? ReadWeight(string label)
        {
            Console.WriteLine($"\n{label}");

            Console.Write("Enter numeric value: ");
            if (!double.TryParse(Console.ReadLine(), out double value))
            {
                Console.WriteLine("Invalid numeric value.");
                return null;
            }

            Console.Write("Enter unit (Kilogram/Gram/Pound): ");
            string? unitInput = Console.ReadLine();

            if (!Enum.TryParse(unitInput, true, out WeightUnit unit))
            {
                Console.WriteLine("Invalid unit type.");
                return null;
            }

            try
            {
                return new QuantityWeight(value, unit);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Invalid input: {ex.Message}");
                return null;
            }
        }
    }
}