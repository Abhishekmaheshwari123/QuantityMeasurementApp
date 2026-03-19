using System.Collections.Generic;
using ModelLayer.Entities;

namespace RepositoryLayer.Interfaces
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityMeasurementEntity entity);
        IReadOnlyList<QuantityMeasurementEntity> GetAllMeasurements();
    }
}