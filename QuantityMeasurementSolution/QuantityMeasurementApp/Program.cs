using System;
using ModelLayer.DTOs;
using BusinessLayer.Interfaces;
using RepositoryLayer.Interfaces;
using BusinessLayer.Services;
using RepositoryLayer.Repositories;
using QuantityMeasurement.Domain.Services;
using QuantityMeasurementApp.Controllers;
using QuantityMeasurementApp.Configuration;
using RepositoryLayer.Configuration;

namespace QuantityMeasurementApp
{
    internal static class Program
    {
        private static void Main()
        {
            var appConfig = new ApplicationConfig(
                useDatabaseRepository: true,
                connectionString: "Server=localhost,1433;Database=QuantityMeasurementDb;User Id=sa;Password=Admin@123;TrustServerCertificate=True;"
            );

            IQuantityMeasurementRepository repository = CreateRepository(appConfig);
            IQuantityMeasurementService service = new QuantityMeasurementService(repository);
            var controller = new QuantityMeasurementController(service);

            Console.WriteLine("=== Quantity Measurement Application (UC16 + UC17 API) ===");
            Console.WriteLine($"Repository: {repository.GetRepositoryInfo()}");
            Console.WriteLine($"Pool Info: {repository.GetPoolStatistics()}");
            Console.WriteLine();

            try
            {
                RunApplication(controller);
            }
            finally
            {
                repository.ReleaseResources();
            }
        }

        private static IQuantityMeasurementRepository CreateRepository(ApplicationConfig appConfig)
        {
            if (appConfig.UseDatabaseRepository)
            {
                return new QuantityMeasurementDatabaseRepository(
                    new DatabaseConfig(appConfig.ConnectionString));
            }

            return new QuantityMeasurementCacheRepository();
        }

        private static void RunApplication(QuantityMeasurementController controller)
        {
            Console.WriteLine("1. Length Operations");
            Console.WriteLine("2. Weight Operations");
            Console.WriteLine("3. Volume Operations");
            Console.WriteLine("4. Temperature Operations");
            Console.Write("\nChoose category: ");

            string? categoryChoice = Console.ReadLine();

            switch (categoryChoice)
            {
                case "1":
                    HandleStandardOperations(controller, "Length");
                    break;
                case "2":
                    HandleStandardOperations(controller, "Weight");
                    break;
                case "3":
                    HandleStandardOperations(controller, "Volume");
                    break;
                case "4":
                    HandleTemperatureOperations(controller);
                    break;
                default:
                    Console.WriteLine("Invalid category.");
                    break;
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        private static void HandleStandardOperations(
            QuantityMeasurementController controller,
            string measurementType)
        {
            Console.WriteLine("\n1. Equality");
            Console.WriteLine("2. Conversion");
            Console.WriteLine("3. Addition");
            Console.WriteLine("4. Addition with Target Unit");
            Console.WriteLine("5. Subtraction");
            Console.WriteLine("6. Division");
            Console.Write("Choose operation: ");

            string? operationChoice = Console.ReadLine();

            switch (operationChoice)
            {
                case "1":
                {
                    var first = ReadQuantityDto($"Enter first {measurementType.ToLower()}", measurementType);
                    var second = ReadQuantityDto($"Enter second {measurementType.ToLower()}", measurementType);

                    if (first == null || second == null)
                        return;

                    try
                    {
                        bool result = controller.PerformComparison(first, second);
                        Console.WriteLine($"\nEqual: {result}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Comparison failed: {ex.Message}");
                    }

                    break;
                }

                case "2":
                {
                    var source = ReadQuantityDto($"Enter {measurementType.ToLower()} to convert", measurementType);
                    if (source == null)
                        return;

                    Console.Write($"Target unit ({GetSupportedUnits(measurementType)}): ");
                    string? targetUnit = Console.ReadLine();

                    try
                    {
                        var result = controller.PerformConversion(source, targetUnit ?? string.Empty);
                        Console.WriteLine($"\nConverted: {result}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Conversion failed: {ex.Message}");
                    }

                    break;
                }

                case "3":
                {
                    var first = ReadQuantityDto($"Enter first {measurementType.ToLower()}", measurementType);
                    var second = ReadQuantityDto($"Enter second {measurementType.ToLower()}", measurementType);

                    if (first == null || second == null)
                        return;

                    try
                    {
                        var result = controller.PerformAddition(first, second);
                        Console.WriteLine($"\nResult: {result}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Addition failed: {ex.Message}");
                    }

                    break;
                }

                case "4":
                {
                    var first = ReadQuantityDto($"Enter first {measurementType.ToLower()}", measurementType);
                    var second = ReadQuantityDto($"Enter second {measurementType.ToLower()}", measurementType);

                    if (first == null || second == null)
                        return;

                    Console.Write($"Target unit ({GetSupportedUnits(measurementType)}): ");
                    string? targetUnit = Console.ReadLine();

                    try
                    {
                        var result = controller.PerformAddition(first, second, targetUnit);
                        Console.WriteLine($"\nResult: {result}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Addition failed: {ex.Message}");
                    }

                    break;
                }

                case "5":
                {
                    var first = ReadQuantityDto($"Enter first {measurementType.ToLower()}", measurementType);
                    var second = ReadQuantityDto($"Enter second {measurementType.ToLower()}", measurementType);

                    if (first == null || second == null)
                        return;

                    Console.Write($"Target unit ({GetSupportedUnits(measurementType)}) or leave blank for implicit: ");
                    string? targetUnit = Console.ReadLine();

                    try
                    {
                        var result = controller.PerformSubtraction(first, second, string.IsNullOrWhiteSpace(targetUnit) ? null : targetUnit);
                        Console.WriteLine($"\nResult: {result}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Subtraction failed: {ex.Message}");
                    }

                    break;
                }

                case "6":
                {
                    var first = ReadQuantityDto($"Enter dividend {measurementType.ToLower()}", measurementType);
                    var second = ReadQuantityDto($"Enter divisor {measurementType.ToLower()}", measurementType);

                    if (first == null || second == null)
                        return;

                    try
                    {
                        double result = controller.PerformDivision(first, second);
                        Console.WriteLine($"\nRatio: {result}");
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

        private static void HandleTemperatureOperations(QuantityMeasurementController controller)
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
                    var first = ReadQuantityDto("Enter first temperature", "Temperature");
                    var second = ReadQuantityDto("Enter second temperature", "Temperature");

                    if (first == null || second == null)
                        return;

                    try
                    {
                        bool result = controller.PerformComparison(first, second);
                        Console.WriteLine($"\nEqual: {result}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Comparison failed: {ex.Message}");
                    }

                    break;
                }

                case "2":
                {
                    var source = ReadQuantityDto("Enter temperature to convert", "Temperature");
                    if (source == null)
                        return;

                    Console.Write("Target unit (Celsius/Fahrenheit/Kelvin): ");
                    string? targetUnit = Console.ReadLine();

                    try
                    {
                        var result = controller.PerformConversion(source, targetUnit ?? string.Empty);
                        Console.WriteLine($"\nConverted: {result}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Conversion failed: {ex.Message}");
                    }

                    break;
                }

                case "3":
                {
                    var first = ReadQuantityDto("Enter first temperature", "Temperature");
                    var second = ReadQuantityDto("Enter second temperature", "Temperature");

                    if (first == null || second == null)
                        return;

                    try
                    {
                        var result = controller.PerformAddition(first, second);
                        Console.WriteLine($"\nResult: {result}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Addition failed: {ex.Message}");
                    }

                    break;
                }

                case "4":
                {
                    var first = ReadQuantityDto("Enter first temperature", "Temperature");
                    var second = ReadQuantityDto("Enter second temperature", "Temperature");

                    if (first == null || second == null)
                        return;

                    try
                    {
                        var result = controller.PerformSubtraction(first, second);
                        Console.WriteLine($"\nResult: {result}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Subtraction failed: {ex.Message}");
                    }

                    break;
                }

                case "5":
                {
                    var first = ReadQuantityDto("Enter dividend temperature", "Temperature");
                    var second = ReadQuantityDto("Enter divisor temperature", "Temperature");

                    if (first == null || second == null)
                        return;

                    try
                    {
                        double result = controller.PerformDivision(first, second);
                        Console.WriteLine($"\nRatio: {result}");
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

        private static QuantityDto? ReadQuantityDto(string label, string measurementType)
        {
            Console.WriteLine($"\n{label}");

            Console.Write("Enter numeric value: ");
            if (!double.TryParse(Console.ReadLine(), out double value))
            {
                Console.WriteLine("Invalid number.");
                return null;
            }

            Console.Write($"Enter unit ({GetSupportedUnits(measurementType)}): ");
            string? unit = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(unit))
            {
                Console.WriteLine("Invalid unit.");
                return null;
            }

            try
            {
                return new QuantityDto(value, unit, measurementType);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        private static string GetSupportedUnits(string measurementType)
        {
            return measurementType.ToLowerInvariant() switch
            {
                "length" => "Feet/Inches/Yards/Centimeters",
                "weight" => "Gram/Kilogram/Pound/Tonne",
                "volume" => "Litre/Millilitre/Gallon",
                "temperature" => "Celsius/Fahrenheit/Kelvin",
                _ => "Unsupported"
            };
        }
    }
}