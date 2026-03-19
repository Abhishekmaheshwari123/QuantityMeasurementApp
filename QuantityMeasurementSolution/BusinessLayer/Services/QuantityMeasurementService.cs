using System;
using System.Globalization;
using ModelLayer.DTOs;
using ModelLayer.Entities;
using QuantityMeasurement.Domain.Enums;
using QuantityMeasurement.Domain.Exceptions;
using BusinessLayer.Interfaces;
using RepositoryLayer.Interfaces;
using ModelLayer.Models;
using QuantityMeasurement.Domain.ValueObjects;

namespace BusinessLayer.Services
{
    public sealed class QuantityMeasurementService : IQuantityMeasurementService
    {
        private readonly IQuantityMeasurementRepository _repository;

        public QuantityMeasurementService(IQuantityMeasurementRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public bool Compare(QuantityDto firstQuantity, QuantityDto secondQuantity)
        {
            try
            {
                ValidateSameMeasurementType(firstQuantity, secondQuantity);

                bool result = NormalizeMeasurementType(firstQuantity.MeasurementType) switch
                {
                    "length" => CompareInternal<LengthUnit>(firstQuantity, secondQuantity),
                    "weight" => CompareInternal<WeightUnit>(firstQuantity, secondQuantity),
                    "volume" => CompareInternal<VolumeUnit>(firstQuantity, secondQuantity),
                    "temperature" => CompareInternal<TemperatureUnit>(firstQuantity, secondQuantity),
                    _ => throw new QuantityMeasurementException("Unsupported measurement type.")
                };

                _repository.Save(new QuantityMeasurementEntity(
                    operation: "COMPARE",
                    firstValue: firstQuantity.Value,
                    firstUnit: firstQuantity.Unit,
                    firstMeasurementType: firstQuantity.MeasurementType,
                    secondValue: secondQuantity.Value,
                    secondUnit: secondQuantity.Unit,
                    secondMeasurementType: secondQuantity.MeasurementType,
                    resultText: result ? "Equal" : "Not Equal",
                    isSuccess: true));

                return result;
            }
            catch (Exception ex)
            {
                throw CreateAndStoreFailure("COMPARE", firstQuantity, secondQuantity, ex);
            }
        }

        public QuantityDto Convert(QuantityDto sourceQuantity, string targetUnit)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(targetUnit))
                    throw new QuantityMeasurementException("Target unit is required.");

                QuantityDto result = NormalizeMeasurementType(sourceQuantity.MeasurementType) switch
                {
                    "length" => ConvertInternal<LengthUnit>(sourceQuantity, targetUnit),
                    "weight" => ConvertInternal<WeightUnit>(sourceQuantity, targetUnit),
                    "volume" => ConvertInternal<VolumeUnit>(sourceQuantity, targetUnit),
                    "temperature" => ConvertInternal<TemperatureUnit>(sourceQuantity, targetUnit),
                    _ => throw new QuantityMeasurementException("Unsupported measurement type.")
                };

                _repository.Save(new QuantityMeasurementEntity(
                    operation: "CONVERT",
                    firstValue: sourceQuantity.Value,
                    firstUnit: sourceQuantity.Unit,
                    firstMeasurementType: sourceQuantity.MeasurementType,
                    resultValue: result.Value,
                    resultUnit: result.Unit,
                    resultMeasurementType: result.MeasurementType,
                    resultText: result.ToString(),
                    isSuccess: true));

                return result;
            }
            catch (Exception ex)
            {
                throw CreateAndStoreFailure("CONVERT", sourceQuantity, null, ex);
            }
        }

        public QuantityDto Add(QuantityDto firstQuantity, QuantityDto secondQuantity, string? targetUnit = null)
        {
            try
            {
                ValidateSameMeasurementType(firstQuantity, secondQuantity);

                QuantityDto result = NormalizeMeasurementType(firstQuantity.MeasurementType) switch
                {
                    "length" => AddInternal<LengthUnit>(firstQuantity, secondQuantity, targetUnit),
                    "weight" => AddInternal<WeightUnit>(firstQuantity, secondQuantity, targetUnit),
                    "volume" => AddInternal<VolumeUnit>(firstQuantity, secondQuantity, targetUnit),
                    "temperature" => AddInternal<TemperatureUnit>(firstQuantity, secondQuantity, targetUnit),
                    _ => throw new QuantityMeasurementException("Unsupported measurement type.")
                };

                _repository.Save(new QuantityMeasurementEntity(
                    operation: "ADD",
                    firstValue: firstQuantity.Value,
                    firstUnit: firstQuantity.Unit,
                    firstMeasurementType: firstQuantity.MeasurementType,
                    secondValue: secondQuantity.Value,
                    secondUnit: secondQuantity.Unit,
                    secondMeasurementType: secondQuantity.MeasurementType,
                    resultValue: result.Value,
                    resultUnit: result.Unit,
                    resultMeasurementType: result.MeasurementType,
                    resultText: result.ToString(),
                    isSuccess: true));

                return result;
            }
            catch (Exception ex)
            {
                throw CreateAndStoreFailure("ADD", firstQuantity, secondQuantity, ex);
            }
        }

        public QuantityDto Subtract(QuantityDto firstQuantity, QuantityDto secondQuantity, string? targetUnit = null)
        {
            try
            {
                ValidateSameMeasurementType(firstQuantity, secondQuantity);

                QuantityDto result = NormalizeMeasurementType(firstQuantity.MeasurementType) switch
                {
                    "length" => SubtractInternal<LengthUnit>(firstQuantity, secondQuantity, targetUnit),
                    "weight" => SubtractInternal<WeightUnit>(firstQuantity, secondQuantity, targetUnit),
                    "volume" => SubtractInternal<VolumeUnit>(firstQuantity, secondQuantity, targetUnit),
                    "temperature" => SubtractInternal<TemperatureUnit>(firstQuantity, secondQuantity, targetUnit),
                    _ => throw new QuantityMeasurementException("Unsupported measurement type.")
                };

                _repository.Save(new QuantityMeasurementEntity(
                    operation: "SUBTRACT",
                    firstValue: firstQuantity.Value,
                    firstUnit: firstQuantity.Unit,
                    firstMeasurementType: firstQuantity.MeasurementType,
                    secondValue: secondQuantity.Value,
                    secondUnit: secondQuantity.Unit,
                    secondMeasurementType: secondQuantity.MeasurementType,
                    resultValue: result.Value,
                    resultUnit: result.Unit,
                    resultMeasurementType: result.MeasurementType,
                    resultText: result.ToString(),
                    isSuccess: true));

                return result;
            }
            catch (Exception ex)
            {
                throw CreateAndStoreFailure("SUBTRACT", firstQuantity, secondQuantity, ex);
            }
        }

        public double Divide(QuantityDto firstQuantity, QuantityDto secondQuantity)
        {
            try
            {
                ValidateSameMeasurementType(firstQuantity, secondQuantity);

                double result = NormalizeMeasurementType(firstQuantity.MeasurementType) switch
                {
                    "length" => DivideInternal<LengthUnit>(firstQuantity, secondQuantity),
                    "weight" => DivideInternal<WeightUnit>(firstQuantity, secondQuantity),
                    "volume" => DivideInternal<VolumeUnit>(firstQuantity, secondQuantity),
                    "temperature" => DivideInternal<TemperatureUnit>(firstQuantity, secondQuantity),
                    _ => throw new QuantityMeasurementException("Unsupported measurement type.")
                };

                _repository.Save(new QuantityMeasurementEntity(
                    operation: "DIVIDE",
                    firstValue: firstQuantity.Value,
                    firstUnit: firstQuantity.Unit,
                    firstMeasurementType: firstQuantity.MeasurementType,
                    secondValue: secondQuantity.Value,
                    secondUnit: secondQuantity.Unit,
                    secondMeasurementType: secondQuantity.MeasurementType,
                    resultText: result.ToString(CultureInfo.InvariantCulture),
                    isSuccess: true));

                return result;
            }
            catch (Exception ex)
            {
                throw CreateAndStoreFailure("DIVIDE", firstQuantity, secondQuantity, ex);
            }
        }

        private static bool CompareInternal<TUnit>(QuantityDto first, QuantityDto second)
            where TUnit : struct, Enum
        {
            var firstQuantity = CreateQuantity<TUnit>(first);
            var secondQuantity = CreateQuantity<TUnit>(second);

            return firstQuantity.Equals(secondQuantity);
        }

        private static QuantityDto ConvertInternal<TUnit>(QuantityDto source, string targetUnit)
            where TUnit : struct, Enum
        {
            var quantity = CreateQuantity<TUnit>(source);
            TUnit parsedTargetUnit = ParseUnit<TUnit>(targetUnit);

            var result = quantity.ConvertTo(parsedTargetUnit);
            return MapToDto(result);
        }

        private static QuantityDto AddInternal<TUnit>(QuantityDto first, QuantityDto second, string? targetUnit)
            where TUnit : struct, Enum
        {
            var left = CreateQuantity<TUnit>(first);
            var right = CreateQuantity<TUnit>(second);

            var result = string.IsNullOrWhiteSpace(targetUnit)
                ? left.Add(right)
                : left.Add(right, ParseUnit<TUnit>(targetUnit));

            return MapToDto(result);
        }

        private static QuantityDto SubtractInternal<TUnit>(QuantityDto first, QuantityDto second, string? targetUnit)
            where TUnit : struct, Enum
        {
            var left = CreateQuantity<TUnit>(first);
            var right = CreateQuantity<TUnit>(second);

            var result = string.IsNullOrWhiteSpace(targetUnit)
                ? left.Subtract(right)
                : left.Subtract(right, ParseUnit<TUnit>(targetUnit));

            return MapToDto(result);
        }

        private static double DivideInternal<TUnit>(QuantityDto first, QuantityDto second)
            where TUnit : struct, Enum
        {
            var left = CreateQuantity<TUnit>(first);
            var right = CreateQuantity<TUnit>(second);

            return left.Divide(right);
        }

        private static Quantity<TUnit> CreateQuantity<TUnit>(QuantityDto dto)
            where TUnit : struct, Enum
        {
            var model = new QuantityModel<TUnit>(dto.Value, ParseUnit<TUnit>(dto.Unit));
            return new Quantity<TUnit>(model.Value, model.Unit);
        }

        private static TUnit ParseUnit<TUnit>(string unitText)
            where TUnit : struct, Enum
        {
            if (!Enum.TryParse(unitText, true, out TUnit parsedUnit) ||
                !Enum.IsDefined(typeof(TUnit), parsedUnit))
            {
                throw new QuantityMeasurementException($"Invalid unit '{unitText}'.");
            }

            return parsedUnit;
        }

        private static QuantityDto MapToDto<TUnit>(Quantity<TUnit> quantity)
            where TUnit : struct, Enum
        {
            return new QuantityDto(
                quantity.Value,
                quantity.Unit.ToString(),
                GetMeasurementType<TUnit>());
        }

        private static string GetMeasurementType<TUnit>()
            where TUnit : struct, Enum
        {
            return typeof(TUnit).Name.Replace("Unit", string.Empty);
        }

        private static void ValidateSameMeasurementType(QuantityDto first, QuantityDto second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));

            if (second == null)
                throw new ArgumentNullException(nameof(second));

            if (!string.Equals(
                    NormalizeMeasurementType(first.MeasurementType),
                    NormalizeMeasurementType(second.MeasurementType),
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new QuantityMeasurementException("Operations are allowed only within the same measurement category.");
            }
        }

        private static string NormalizeMeasurementType(string measurementType)
        {
            if (string.IsNullOrWhiteSpace(measurementType))
                throw new QuantityMeasurementException("Measurement type is required.");

            return measurementType.Trim().ToLowerInvariant();
        }

        private QuantityMeasurementException CreateAndStoreFailure(
            string operation,
            QuantityDto first,
            QuantityDto? second,
            Exception ex)
        {
            _repository.Save(new QuantityMeasurementEntity(
                operation: operation,
                firstValue: first.Value,
                firstUnit: first.Unit,
                firstMeasurementType: first.MeasurementType,
                secondValue: second?.Value,
                secondUnit: second?.Unit,
                secondMeasurementType: second?.MeasurementType,
                isSuccess: false,
                errorMessage: ex.Message));

            return ex as QuantityMeasurementException
                   ?? new QuantityMeasurementException($"{operation} operation failed: {ex.Message}", ex);
        }
    }
}