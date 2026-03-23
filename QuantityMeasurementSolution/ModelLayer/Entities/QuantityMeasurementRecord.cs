using System;
using System.Collections.Generic;

namespace QuantityMeasurement.ModelLayer.Entities
{
    /// <summary>
    /// ORM record mapped to dbo.QuantityMeasurements for persisted quantity operations.
    /// </summary>
    public sealed class QuantityMeasurementRecord
    {
        public long Id { get; set; }
        public string Operation { get; set; } = string.Empty;

        public double FirstValue { get; set; }
        public string FirstUnit { get; set; } = string.Empty;
        public string FirstMeasurementType { get; set; } = string.Empty;

        public double? SecondValue { get; set; }
        public string? SecondUnit { get; set; }
        public string? SecondMeasurementType { get; set; }

        public double? ResultValue { get; set; }
        public string? ResultUnit { get; set; }
        public string? ResultMeasurementType { get; set; }

        public string? ResultText { get; set; }
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public ICollection<QuantityMeasurementHistoryRecord> HistoryRecords { get; set; }
            = new List<QuantityMeasurementHistoryRecord>();
    }
}
