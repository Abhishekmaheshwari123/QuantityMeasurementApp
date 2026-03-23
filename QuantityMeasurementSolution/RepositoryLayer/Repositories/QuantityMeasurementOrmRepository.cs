using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Entities;
using QuantityMeasurement.Domain.Exceptions;
using QuantityMeasurement.RepositoryLayer.Data;
using QuantityMeasurement.ModelLayer.Entities;
using RepositoryLayer.Interfaces;

namespace QuantityMeasurement.RepositoryLayer.Repositories
{
    /// <summary>
    /// EF Core implementation of quantity measurement persistence operations.
    /// </summary>
    public sealed class QuantityMeasurementOrmRepository : IQuantityMeasurementRepository
    {
        private readonly QuantityMeasurementDbContext _dbContext;

        public QuantityMeasurementOrmRepository(QuantityMeasurementDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var record = new QuantityMeasurementRecord
                {
                    Operation = entity.Operation,
                    FirstValue = entity.FirstValue,
                    FirstUnit = entity.FirstUnit,
                    FirstMeasurementType = entity.FirstMeasurementType,
                    SecondValue = entity.SecondValue,
                    SecondUnit = entity.SecondUnit,
                    SecondMeasurementType = entity.SecondMeasurementType,
                    ResultValue = entity.ResultValue,
                    ResultUnit = entity.ResultUnit,
                    ResultMeasurementType = entity.ResultMeasurementType,
                    ResultText = entity.ResultText,
                    IsSuccess = entity.IsSuccess,
                    ErrorMessage = entity.ErrorMessage
                };

                _dbContext.QuantityMeasurements.Add(record);
                _dbContext.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new DatabaseException("Failed to save quantity measurement through ORM.", ex);
            }
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetAllMeasurements()
        {
            try
            {
                return _dbContext.QuantityMeasurements
                    .OrderByDescending(x => x.CreatedAtUtc)
                    .AsNoTracking()
                    .Select(MapEntity)
                    .ToList()
                    .AsReadOnly();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to retrieve measurements from ORM store.", ex);
            }
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetMeasurementsByOperation(string operation)
        {
            try
            {
                return _dbContext.QuantityMeasurements
                    .Where(x => x.Operation == operation)
                    .OrderByDescending(x => x.CreatedAtUtc)
                    .AsNoTracking()
                    .Select(MapEntity)
                    .ToList()
                    .AsReadOnly();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to retrieve measurements by operation from ORM store.", ex);
            }
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetMeasurementsByMeasurementType(string measurementType)
        {
            try
            {
                return _dbContext.QuantityMeasurements
                    .Where(x => x.FirstMeasurementType == measurementType)
                    .OrderByDescending(x => x.CreatedAtUtc)
                    .AsNoTracking()
                    .Select(MapEntity)
                    .ToList()
                    .AsReadOnly();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to retrieve measurements by type from ORM store.", ex);
            }
        }

        public int GetTotalCount()
        {
            try
            {
                return _dbContext.QuantityMeasurements.Count();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to count ORM measurements.", ex);
            }
        }

        public void DeleteAll()
        {
            try
            {
                var all = _dbContext.QuantityMeasurements.ToList();
                _dbContext.QuantityMeasurements.RemoveRange(all);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to delete ORM measurements.", ex);
            }
        }

        public string GetRepositoryInfo()
        {
            return "SQL Server Repository (EF Core ORM)";
        }

        public string GetPoolStatistics()
        {
            return "EF Core uses the configured SQL client pooling behavior.";
        }

        public void ReleaseResources()
        {
            _dbContext.Dispose();
        }

        private static QuantityMeasurementEntity MapEntity(QuantityMeasurementRecord record)
        {
            return new QuantityMeasurementEntity(
                operation: record.Operation,
                firstValue: record.FirstValue,
                firstUnit: record.FirstUnit,
                firstMeasurementType: record.FirstMeasurementType,
                secondValue: record.SecondValue,
                secondUnit: record.SecondUnit,
                secondMeasurementType: record.SecondMeasurementType,
                resultValue: record.ResultValue,
                resultUnit: record.ResultUnit,
                resultMeasurementType: record.ResultMeasurementType,
                resultText: record.ResultText,
                isSuccess: record.IsSuccess,
                errorMessage: record.ErrorMessage
            );
        }
    }
}
