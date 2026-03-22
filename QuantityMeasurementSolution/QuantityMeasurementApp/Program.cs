using System;
using QuantityMeasurement.Domain.Enums;
using QuantityMeasurement.Domain.ValueObjects;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// Console entry point for Quantity Measurement Application.
    /// Supports Length, Weight, Volume and Temperature operations using generic Quantity<TUnit>.
    /// </summary>
    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine("=== Quantity Measurement Application (UC14) ===\n");

            Console.WriteLine("1. Length Operations");
            Console.WriteLine("2. Weight Operations");
            Console.WriteLine("3. Volume Operations");
            Console.WriteLine("4. Temperature Operations");
            Console.Write("\nChoose category: ");

            string? category = Console.ReadLine();

            switch (category)
            {
                case "1":
                    HandleLengthOperations();
                    break;
                case "2":
                    HandleWeightOperations();
                    break;
                case "3":
                    HandleVolumeOperations();
                    break;
                case "4":
                    HandleTemperatureOperations();
                    break;
                default:
                    Console.WriteLine("Invalid category.");
                    break;
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        // ================= LENGTH =================

        private static void HandleLengthOperations()
        {
            Console.WriteLine("\n1. Equality");
            Console.WriteLine("2. Conversion");
            Console.WriteLine("3. Addition");
            Console.WriteLine("4. Addition with Target Unit");
            Console.WriteLine("5. Subtraction");
            Console.WriteLine("6. Division");
            Console.Write("Choose operation: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    {
                        var l1 = ReadQuantity<LengthUnit>("Enter first length");
                        var l2 = ReadQuantity<LengthUnit>("Enter second length");

                        if (l1 == null || l2 == null) return;
                        
                        Console.WriteLine($"\nEqual: {l1.Equals(l2)}");
                        break;
                    }

                case "2":
                    {
                        var length = ReadQuantity<LengthUnit>("Enter length to convert");
                        if (length == null) return;

                        Console.Write("Target unit (Feet/Inches/Yards/Centimeters): ");
                        if (!Enum.TryParse(Console.ReadLine(), true, out LengthUnit targetLength))
                        {
                            Console.WriteLine("Invalid unit.");
                            return;
                        }

                        Console.WriteLine($"\nConverted: {length.ConvertTo(targetLength)}");
                        break;
                    }

                case "3":
                    {
                        var a = ReadQuantity<LengthUnit>("Enter first length");
                        var b = ReadQuantity<LengthUnit>("Enter second length");

                        if (a == null || b == null) return;

                        Console.WriteLine($"\nResult: {a.Add(b)}");
                        break;
                    }

                case "4":
                    {
                        var x = ReadQuantity<LengthUnit>("Enter first length");
                        var y = ReadQuantity<LengthUnit>("Enter second length");

                        if (x == null || y == null) return;

                        Console.Write("Target unit (Feet/Inches/Yards/Centimeters): ");
                        if (!Enum.TryParse(Console.ReadLine(), true, out LengthUnit targetAdd))
                        {
                            Console.WriteLine("Invalid unit.");
                            return;
                        }

                        Console.WriteLine($"\nResult: {x.Add(y, targetAdd)}");
                        break;
                    }

                case "5":
                    {
                        var x = ReadQuantity<LengthUnit>("Enter first length");
                        var y = ReadQuantity<LengthUnit>("Enter second length");

                        if (x == null || y == null) return;

                        Console.Write("Target unit (Feet/Inches/Yards/Centimeters) or leave blank for implicit: ");
                        string? targetInput = Console.ReadLine();

                        try
                        {
                            if (string.IsNullOrWhiteSpace(targetInput))
                            {
                                Console.WriteLine($"\nResult: {x.Subtract(y)}");
                            }
                            else if (Enum.TryParse(targetInput, true, out LengthUnit targetSubtract))
                            {
                                Console.WriteLine($"\nResult: {x.Subtract(y, targetSubtract)}");
                            }
                            else
                            {
                                Console.WriteLine("Invalid target unit.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Subtraction failed: {ex.Message}");
                        }

                        break;
                    }

                case "6":
                    {
                        var x = ReadQuantity<LengthUnit>("Enter dividend length");
                        var y = ReadQuantity<LengthUnit>("Enter divisor length");

                        if (x == null || y == null) return;

                        try
                        {
                            Console.WriteLine($"\nRatio: {x.Divide(y)}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Division failed: {ex.Message}");
                        }

                        break;
                    }

                default:
                    Console.WriteLine("Invalid operation.");
                    break;
            }
        }

        // ================= WEIGHT =================

        private static void HandleWeightOperations()
        {
            Console.WriteLine("\n1. Equality");
            Console.WriteLine("2. Conversion");
            Console.WriteLine("3. Addition");
            Console.WriteLine("4. Addition with Target Unit");
            Console.WriteLine("5. Subtraction");
            Console.WriteLine("6. Division");
            Console.Write("Choose operation: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    {
                        var w1 = ReadQuantity<WeightUnit>("Enter first weight");
                        var w2 = ReadQuantity<WeightUnit>("Enter second weight");

                        if (w1 == null || w2 == null) return;
                        
                        Console.WriteLine($"\nEqual: {w1.Equals(w2)}");
                        break;
                    }

                case "2":
                    {
                        var weight = ReadQuantity<WeightUnit>("Enter weight to convert");
                        if (weight == null) return;

                        Console.Write("Target unit (Gram/Kilogram/Pound/Tonne): ");
                        if (!Enum.TryParse(Console.ReadLine(), true, out WeightUnit targetWeight))
                        {
                            Console.WriteLine("Invalid unit.");
                            return;
                        }

                        Console.WriteLine($"\nConverted: {weight.ConvertTo(targetWeight)}");
                        break;
                    }

                case "3":
                    {
                        var a = ReadQuantity<WeightUnit>("Enter first weight");
                        var b = ReadQuantity<WeightUnit>("Enter second weight");

                        if (a == null || b == null) return;

                        Console.WriteLine($"\nResult: {a.Add(b)}");
                        break;
                    }

                case "4":
                    {
                        var x = ReadQuantity<WeightUnit>("Enter first weight");
                        var y = ReadQuantity<WeightUnit>("Enter second weight");

                        if (x == null || y == null) return;

                        Console.Write("Target unit (Gram/Kilogram/Pound/Tonne): ");
                        if (!Enum.TryParse(Console.ReadLine(), true, out WeightUnit targetAdd))
                        {
                            Console.WriteLine("Invalid unit.");
                            return;
                        }

                        Console.WriteLine($"\nResult: {x.Add(y, targetAdd)}");
                        break;
                    }

                case "5":
                    {
                        var x = ReadQuantity<WeightUnit>("Enter first weight");
                        var y = ReadQuantity<WeightUnit>("Enter second weight");

                        if (x == null || y == null) return;

                        Console.Write("Target unit (Gram/Kilogram/Pound/Tonne) or leave blank for implicit: ");
                        string? targetInput = Console.ReadLine();

                        try
                        {
                            if (string.IsNullOrWhiteSpace(targetInput))
                            {
                                Console.WriteLine($"\nResult: {x.Subtract(y)}");
                            }
                            else if (Enum.TryParse(targetInput, true, out WeightUnit targetSubtract))
                            {
                                Console.WriteLine($"\nResult: {x.Subtract(y, targetSubtract)}");
                            }
                            else
                            {
                                Console.WriteLine("Invalid target unit.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Subtraction failed: {ex.Message}");
                        }

                        break;
                    }

                case "6":
                    {
                        var x = ReadQuantity<WeightUnit>("Enter dividend weight");
                        var y = ReadQuantity<WeightUnit>("Enter divisor weight");

                        if (x == null || y == null) return;

                        try
                        {
                            Console.WriteLine($"\nRatio: {x.Divide(y)}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Division failed: {ex.Message}");
                        }

                        break;
                    }

                default:
                    Console.WriteLine("Invalid operation.");
                    break;
            }
        }

        // ================= VOLUME =================

        private static void HandleVolumeOperations()
        {
            Console.WriteLine("\n1. Equality");
            Console.WriteLine("2. Conversion");
            Console.WriteLine("3. Addition");
            Console.WriteLine("4. Addition with Target Unit");
            Console.WriteLine("5. Subtraction");
            Console.WriteLine("6. Division");
            Console.Write("Choose operation: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    {
                        var v1 = ReadQuantity<VolumeUnit>("Enter first volume");
                        var v2 = ReadQuantity<VolumeUnit>("Enter second volume");

                        if (v1 == null || v2 == null) return;

                        Console.WriteLine($"\nEqual: {v1.Equals(v2)}");
                        break;
                    }

                case "2":
                    {
                        var volume = ReadQuantity<VolumeUnit>("Enter volume to convert");
                        if (volume == null) return;

                        Console.Write("Target unit (Litre/Millilitre/Gallon): ");
                        if (!Enum.TryParse(Console.ReadLine(), true, out VolumeUnit target))
                        {
                            Console.WriteLine("Invalid unit.");
                            return;
                        }

                        Console.WriteLine($"\nConverted: {volume.ConvertTo(target)}");
                        break;
                    }

                case "3":
                    {
                        var a = ReadQuantity<VolumeUnit>("Enter first volume");
                        var b = ReadQuantity<VolumeUnit>("Enter second volume");

                        if (a == null || b == null) return;

                        Console.WriteLine($"\nResult: {a.Add(b)}");
                        break;
                    }

                case "4":
                    {
                        var x = ReadQuantity<VolumeUnit>("Enter first volume");
                        var y = ReadQuantity<VolumeUnit>("Enter second volume");

                        if (x == null || y == null) return;

                        Console.Write("Target unit (Litre/Millilitre/Gallon): ");
                        if (!Enum.TryParse(Console.ReadLine(), true, out VolumeUnit targetAdd))
                        {
                            Console.WriteLine("Invalid unit.");
                            return;
                        }

                        Console.WriteLine($"\nResult: {x.Add(y, targetAdd)}");
                        break;
                    }

                case "5":
                    {
                        var x = ReadQuantity<VolumeUnit>("Enter first volume");
                        var y = ReadQuantity<VolumeUnit>("Enter second volume");

                        if (x == null || y == null) return;

                        Console.Write("Target unit (Litre/Millilitre/Gallon) or leave blank for implicit: ");
                        string? targetInput = Console.ReadLine();

                        try
                        {
                            if (string.IsNullOrWhiteSpace(targetInput))
                            {
                                Console.WriteLine($"\nResult: {x.Subtract(y)}");
                            }
                            else if (Enum.TryParse(targetInput, true, out VolumeUnit targetSubtract))
                            {
                                Console.WriteLine($"\nResult: {x.Subtract(y, targetSubtract)}");
                            }
                            else
                            {
                                Console.WriteLine("Invalid target unit.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Subtraction failed: {ex.Message}");
                        }

                        break;
                    }

                case "6":
                    {
                        var x = ReadQuantity<VolumeUnit>("Enter dividend volume");
                        var y = ReadQuantity<VolumeUnit>("Enter divisor volume");

                        if (x == null || y == null) return;

                        try
                        {
                            Console.WriteLine($"\nRatio: {x.Divide(y)}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Division failed: {ex.Message}");
                        }

                        break;
                    }
                default:
                    Console.WriteLine("Invalid operation.");
                    break;
            }
        }

        // ================= TEMPERATURE =================

        private static void HandleTemperatureOperations()
        {
            Console.WriteLine("\n1. Equality");
            Console.WriteLine("2. Conversion");
            Console.WriteLine("3. Addition (Unsupported)");
            Console.WriteLine("4. Subtraction (Unsupported)");
            Console.WriteLine("5. Division (Unsupported)");
            Console.Write("Choose operation: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    {
                        var t1 = ReadQuantity<TemperatureUnit>("Enter first temperature");
                        var t2 = ReadQuantity<TemperatureUnit>("Enter second temperature");

                        if (t1 == null || t2 == null) return;
                        Console.WriteLine($"\nEqual: {t1.Equals(t2)}");
                        break;
                    }

                case "2":
                    {
                        var temp = ReadQuantity<TemperatureUnit>("Enter temperature to convert");
                        if (temp == null) return;

                        Console.Write("Target unit (Celsius/Fahrenheit/Kelvin): ");
                        if (!Enum.TryParse(Console.ReadLine(), true, out TemperatureUnit targetTemp))
                        {
                            Console.WriteLine("Invalid unit.");
                            return;
                        }

                        try
                        {
                            Console.WriteLine($"\nConverted: {temp.ConvertTo(targetTemp)}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Conversion failed: {ex.Message}");
                        }

                        break;
                    }

                case "3":
                    {
                        var t1 = ReadQuantity<TemperatureUnit>("Enter first temperature");
                        var t2 = ReadQuantity<TemperatureUnit>("Enter second temperature");

                        if (t1 == null || t2 == null) return;

                        try
                        {
                            Console.WriteLine($"\nResult: {t1.Add(t2)}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Addition failed: {ex.Message}");
                        }

                        break;
                    }

                case "4":
                    {
                        var t1 = ReadQuantity<TemperatureUnit>("Enter first temperature");
                        var t2 = ReadQuantity<TemperatureUnit>("Enter second temperature");

                        if (t1 == null || t2 == null) return;

                        try
                        {
                            Console.WriteLine($"\nResult: {t1.Subtract(t2)}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Subtraction failed: {ex.Message}");
                        }

                        break;
                    }

                case "5":
                    {
                        var t1 = ReadQuantity<TemperatureUnit>("Enter dividend temperature");
                        var t2 = ReadQuantity<TemperatureUnit>("Enter divisor temperature");

                        if (t1 == null || t2 == null) return;

                        try
                        {
                            Console.WriteLine($"\nRatio: {t1.Divide(t2)}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Division failed: {ex.Message}");
                        }

                        break;
                    }

                default:
                    Console.WriteLine("Invalid operation.");
                    break;
            }
        }

        // ================= GENERIC INPUT =================

        private static Quantity<TUnit>? ReadQuantity<TUnit>(string label)
            where TUnit : struct, Enum
        {
            Console.WriteLine($"\n{label}");

            Console.Write("Enter numeric value: ");
            if (!double.TryParse(Console.ReadLine(), out double value))
            {
                Console.WriteLine("Invalid number.");
                return null;
            }

            if (typeof(TUnit) == typeof(LengthUnit))
            {
                Console.Write("Enter unit (Feet/Inches/Yards/Centimeters): ");
            }
            else if (typeof(TUnit) == typeof(WeightUnit))
            {
                Console.Write("Enter unit (Gram/Kilogram/Pound/Tonne): ");
            }
            else if (typeof(TUnit) == typeof(VolumeUnit))
            {
                Console.Write("Enter unit (Litre/Millilitre/Gallon): ");
            }
            else if (typeof(TUnit) == typeof(TemperatureUnit))
            {
                Console.Write("Enter unit (Celsius/Fahrenheit/Kelvin): ");
            }
            else
            {
                Console.WriteLine("Unsupported unit type.");
                return null;
            }

            if (!Enum.TryParse(Console.ReadLine(), true, out TUnit unit))
            {
                Console.WriteLine("Invalid unit.");
                return null;
            }

            try
            {
                return new Quantity<TUnit>(value, unit);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}