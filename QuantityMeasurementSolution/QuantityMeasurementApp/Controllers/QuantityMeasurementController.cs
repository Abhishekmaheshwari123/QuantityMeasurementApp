using System;
using ModelLayer.DTOs;
using BusinessLayer.Interfaces;

namespace QuantityMeasurementApp.Controllers
{
    public sealed class QuantityMeasurementController
    {
        private readonly IQuantityMeasurementService _quantityMeasurementService;

        public QuantityMeasurementController(IQuantityMeasurementService quantityMeasurementService)
        {
            _quantityMeasurementService = quantityMeasurementService
                ?? throw new ArgumentNullException(nameof(quantityMeasurementService));
        }

        public bool PerformComparison(QuantityDto firstQuantity, QuantityDto secondQuantity)
        {
            return _quantityMeasurementService.Compare(firstQuantity, secondQuantity);
        }

        public QuantityDto PerformConversion(QuantityDto sourceQuantity, string targetUnit)
        {
            return _quantityMeasurementService.Convert(sourceQuantity, targetUnit);
        }

        public QuantityDto PerformAddition(
            QuantityDto firstQuantity,
            QuantityDto secondQuantity,
            string? targetUnit = null)
        {
            return _quantityMeasurementService.Add(firstQuantity, secondQuantity, targetUnit);
        }

        public QuantityDto PerformSubtraction(
            QuantityDto firstQuantity,
            QuantityDto secondQuantity,
            string? targetUnit = null)
        {
            return _quantityMeasurementService.Subtract(firstQuantity, secondQuantity, targetUnit);
        }

        public double PerformDivision(QuantityDto firstQuantity, QuantityDto secondQuantity)
        {
            return _quantityMeasurementService.Divide(firstQuantity, secondQuantity);
        }
    }
}