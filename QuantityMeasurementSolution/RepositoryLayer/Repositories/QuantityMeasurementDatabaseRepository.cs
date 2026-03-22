using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using QuantityMeasurement.Domain.Exceptions;
using ModelLayer.Entities;
using RepositoryLayer.Configuration;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Repositories
{
    public sealed class QuantityMeasurementDatabaseRepository : IQuantityMeasurementRepository
    {
        private readonly string _connectionString;

        public QuantityMeasurementDatabaseRepository(DatabaseConfig databaseConfig)
        {
            if (databaseConfig == null)
                throw new ArgumentNullException(nameof(databaseConfig));

            if (string.IsNullOrWhiteSpace(databaseConfig.ConnectionString))
                throw new ArgumentException("Connection string is required.", nameof(databaseConfig));

            _connectionString = databaseConfig.ConnectionString;
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            const string insertMeasurementSql = @"
INSERT INTO dbo.QuantityMeasurements
(
    Operation,
    FirstValue, FirstUnit, FirstMeasurementType,
    SecondValue, SecondUnit, SecondMeasurementType,
    ResultValue, ResultUnit, ResultMeasurementType,
    ResultText, IsSuccess, ErrorMessage
)
VALUES
(
    @Operation,
    @FirstValue, @FirstUnit, @FirstMeasurementType,
    @SecondValue, @SecondUnit, @SecondMeasurementType,
    @ResultValue, @ResultUnit, @ResultMeasurementType,
    @ResultText, @IsSuccess, @ErrorMessage
);

SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

            const string insertHistorySql = @"
INSERT INTO dbo.QuantityMeasurementHistory
(
    MeasurementId,
    ActionType
)
VALUES
(
    @MeasurementId,
    @ActionType
);";

            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();

                using var transaction = connection.BeginTransaction();

                try
                {
                    long measurementId;

                    using (var command = new SqlCommand(insertMeasurementSql, connection, transaction))
                    {
                        AddSaveParameters(command, entity);
                        measurementId = Convert.ToInt64(command.ExecuteScalar());
                    }

                    using (var historyCommand = new SqlCommand(insertHistorySql, connection, transaction))
                    {
                        historyCommand.Parameters.AddWithValue("@MeasurementId", measurementId);
                        historyCommand.Parameters.AddWithValue("@ActionType", "INSERT");
                        historyCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new DatabaseException("Failed to save quantity measurement to database.", ex);
                }
            }
            catch (DatabaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Database save operation failed.", ex);
            }
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetAllMeasurements()
        {
            const string sql = @"
SELECT
    Operation,
    FirstValue, FirstUnit, FirstMeasurementType,
    SecondValue, SecondUnit, SecondMeasurementType,
    ResultValue, ResultUnit, ResultMeasurementType,
    ResultText, IsSuccess, ErrorMessage
FROM dbo.QuantityMeasurements
ORDER BY CreatedAtUtc DESC;";

            return ExecuteReaderList(sql, null);
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetMeasurementsByOperation(string operation)
        {
            const string sql = @"
SELECT
    Operation,
    FirstValue, FirstUnit, FirstMeasurementType,
    SecondValue, SecondUnit, SecondMeasurementType,
    ResultValue, ResultUnit, ResultMeasurementType,
    ResultText, IsSuccess, ErrorMessage
FROM dbo.QuantityMeasurements
WHERE Operation = @Operation
ORDER BY CreatedAtUtc DESC;";

            return ExecuteReaderList(sql, cmd =>
            {
                cmd.Parameters.AddWithValue("@Operation", operation);
            });
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetMeasurementsByMeasurementType(string measurementType)
        {
            const string sql = @"
SELECT
    Operation,
    FirstValue, FirstUnit, FirstMeasurementType,
    SecondValue, SecondUnit, SecondMeasurementType,
    ResultValue, ResultUnit, ResultMeasurementType,
    ResultText, IsSuccess, ErrorMessage
FROM dbo.QuantityMeasurements
WHERE FirstMeasurementType = @MeasurementType
ORDER BY CreatedAtUtc DESC;";

            return ExecuteReaderList(sql, cmd =>
            {
                cmd.Parameters.AddWithValue("@MeasurementType", measurementType);
            });
        }

        public int GetTotalCount()
        {
            const string sql = @"SELECT COUNT(*) FROM dbo.QuantityMeasurements;";

            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(sql, connection);

                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to get total measurement count.", ex);
            }
        }

        public void DeleteAll()
        {
            const string sql = @"DELETE FROM dbo.QuantityMeasurements;";

            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(sql, connection);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to delete all measurements.", ex);
            }
        }

        public string GetRepositoryInfo()
        {
            return "SQL Server Repository (ADO.NET)";
        }

        public string GetPoolStatistics()
        {
            return "ADO.NET connection pooling is enabled by default for identical connection strings.";
        }

        public void ReleaseResources()
        {
            SqlConnection.ClearAllPools();
        }

        private IReadOnlyList<QuantityMeasurementEntity> ExecuteReaderList(
            string sql,
            Action<SqlCommand>? configureCommand)
        {
            var items = new List<QuantityMeasurementEntity>();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(sql, connection);

                configureCommand?.Invoke(command);

                connection.Open();

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    items.Add(MapEntity(reader));
                }

                return items.AsReadOnly();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to retrieve measurements from database.", ex);
            }
        }

        private static void AddSaveParameters(SqlCommand command, QuantityMeasurementEntity entity)
        {
            command.Parameters.AddWithValue("@Operation", entity.Operation);

            command.Parameters.AddWithValue("@FirstValue", entity.FirstValue);
            command.Parameters.AddWithValue("@FirstUnit", entity.FirstUnit);
            command.Parameters.AddWithValue("@FirstMeasurementType", entity.FirstMeasurementType);

            command.Parameters.AddWithValue("@SecondValue", (object?)entity.SecondValue ?? DBNull.Value);
            command.Parameters.AddWithValue("@SecondUnit", (object?)entity.SecondUnit ?? DBNull.Value);
            command.Parameters.AddWithValue("@SecondMeasurementType", (object?)entity.SecondMeasurementType ?? DBNull.Value);

            command.Parameters.AddWithValue("@ResultValue", (object?)entity.ResultValue ?? DBNull.Value);
            command.Parameters.AddWithValue("@ResultUnit", (object?)entity.ResultUnit ?? DBNull.Value);
            command.Parameters.AddWithValue("@ResultMeasurementType", (object?)entity.ResultMeasurementType ?? DBNull.Value);

            command.Parameters.AddWithValue("@ResultText", (object?)entity.ResultText ?? DBNull.Value);
            command.Parameters.AddWithValue("@IsSuccess", entity.IsSuccess);
            command.Parameters.AddWithValue("@ErrorMessage", (object?)entity.ErrorMessage ?? DBNull.Value);
        }

        private static QuantityMeasurementEntity MapEntity(SqlDataReader reader)
        {
            return new QuantityMeasurementEntity(
                operation: reader.GetString(reader.GetOrdinal("Operation")),
                firstValue: reader.GetDouble(reader.GetOrdinal("FirstValue")),
                firstUnit: reader.GetString(reader.GetOrdinal("FirstUnit")),
                firstMeasurementType: reader.GetString(reader.GetOrdinal("FirstMeasurementType")),
                secondValue: reader.IsDBNull(reader.GetOrdinal("SecondValue"))
                    ? null
                    : reader.GetDouble(reader.GetOrdinal("SecondValue")),
                secondUnit: reader.IsDBNull(reader.GetOrdinal("SecondUnit"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("SecondUnit")),
                secondMeasurementType: reader.IsDBNull(reader.GetOrdinal("SecondMeasurementType"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("SecondMeasurementType")),
                resultValue: reader.IsDBNull(reader.GetOrdinal("ResultValue"))
                    ? null
                    : reader.GetDouble(reader.GetOrdinal("ResultValue")),
                resultUnit: reader.IsDBNull(reader.GetOrdinal("ResultUnit"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("ResultUnit")),
                resultMeasurementType: reader.IsDBNull(reader.GetOrdinal("ResultMeasurementType"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("ResultMeasurementType")),
                resultText: reader.IsDBNull(reader.GetOrdinal("ResultText"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("ResultText")),
                isSuccess: reader.GetBoolean(reader.GetOrdinal("IsSuccess")),
                errorMessage: reader.IsDBNull(reader.GetOrdinal("ErrorMessage"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("ErrorMessage"))
            );
        }
    }
}