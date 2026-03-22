using ModelLayer.DTOs;
using ModelLayer.Entities;

namespace BusinessLayer.Interfaces
{
    public interface IQuantityMeasurementService
    {
        bool Compare(QuantityDto firstQuantity, QuantityDto secondQuantity);

        QuantityDto Convert(QuantityDto sourceQuantity, string targetUnit);

        QuantityDto Add(
            QuantityDto firstQuantity,
            QuantityDto secondQuantity,
            string? targetUnit = null);

        QuantityDto Subtract(
            QuantityDto firstQuantity,
            QuantityDto secondQuantity,
            string? targetUnit = null);

        double Divide(QuantityDto firstQuantity, QuantityDto secondQuantity);

        IReadOnlyList<QuantityMeasurementEntity> GetHistoryByOperation(string operation);

        IReadOnlyList<QuantityMeasurementEntity> GetHistoryByMeasurementType(string measurementType);

        IReadOnlyList<QuantityMeasurementEntity> GetErroredHistory();

        int GetOperationCount(string operation);
    }
}