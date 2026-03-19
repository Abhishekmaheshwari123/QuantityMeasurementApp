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
        private static readonly Lazy<QuantityMeasurementCacheRepository> LazyInstance =
            new(() => new QuantityMeasurementCacheRepository());

        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            WriteIndented = true
        };

        private readonly List<QuantityMeasurementEntity> _cache;
        private readonly string _filePath;
        private readonly object _syncLock = new();

        public static QuantityMeasurementCacheRepository Instance => LazyInstance.Value;

        private QuantityMeasurementCacheRepository()
        {
            _filePath = Path.Combine(AppContext.BaseDirectory, "quantity-measurements-history.json");
            _cache = LoadFromDisk();
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            lock (_syncLock)
            {
                _cache.Add(entity);
                PersistToDisk();
            }
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetAllMeasurements()
        {
            lock (_syncLock)
            {
                return _cache.ToList().AsReadOnly();
            }
        }

        private List<QuantityMeasurementEntity> LoadFromDisk()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return new List<QuantityMeasurementEntity>();

                string json = File.ReadAllText(_filePath);
                if (string.IsNullOrWhiteSpace(json))
                    return new List<QuantityMeasurementEntity>();

                return JsonSerializer.Deserialize<List<QuantityMeasurementEntity>>(json, SerializerOptions)
                       ?? new List<QuantityMeasurementEntity>();
            }
            catch
            {
                return new List<QuantityMeasurementEntity>();
            }
        }

        private void PersistToDisk()
        {
            string json = JsonSerializer.Serialize(_cache, SerializerOptions);
            File.WriteAllText(_filePath, json);
        }
    }
}