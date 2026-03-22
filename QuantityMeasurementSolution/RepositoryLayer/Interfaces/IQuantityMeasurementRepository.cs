using System.Collections.Generic;
using ModelLayer.Entities;

namespace RepositoryLayer.Interfaces
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityMeasurementEntity entity);
        IReadOnlyList<QuantityMeasurementEntity> GetAllMeasurements();

        IReadOnlyList<QuantityMeasurementEntity> GetMeasurementsByOperation(string operation);

        IReadOnlyList<QuantityMeasurementEntity> GetMeasurementsByMeasurementType(string measurementType);

        int GetTotalCount();

        void DeleteAll();

        string GetRepositoryInfo();

        string GetPoolStatistics()
        {
            return "Pool statistics not available.";
        }

        void ReleaseResources()
        {
        }
    }
}