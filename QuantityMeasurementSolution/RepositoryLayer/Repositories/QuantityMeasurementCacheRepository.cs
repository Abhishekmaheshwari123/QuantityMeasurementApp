using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ModelLayer.Entities;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Repositories
{
    public sealed class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private readonly List<QuantityMeasurementEntity> _items = new();

        public void Save(QuantityMeasurementEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _items.Add(entity);
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetAllMeasurements()
        {
            return _items.AsReadOnly();
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetMeasurementsByOperation(string operation)
        {
            return _items
                .Where(x => string.Equals(x.Operation, operation, StringComparison.OrdinalIgnoreCase))
                .ToList()
                .AsReadOnly();
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetMeasurementsByMeasurementType(string measurementType)
        {
            return _items
                .Where(x => string.Equals(x.FirstMeasurementType, measurementType, StringComparison.OrdinalIgnoreCase))
                .ToList()
                .AsReadOnly();
        }

        public int GetTotalCount()
        {
            return _items.Count;
        }

        public void DeleteAll()
        {
            _items.Clear();
        }

        public string GetRepositoryInfo()
        {
            return "In-Memory Cache Repository";
        }
    }
}