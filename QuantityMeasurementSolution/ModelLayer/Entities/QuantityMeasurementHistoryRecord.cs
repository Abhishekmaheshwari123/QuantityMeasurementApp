using System;

namespace QuantityMeasurement.ModelLayer.Entities
{
    /// <summary>
    /// ORM record mapped to dbo.QuantityMeasurementHistory for audit entries.
    /// </summary>
    public sealed class QuantityMeasurementHistoryRecord
    {
        public long HistoryId { get; set; }
        public long MeasurementId { get; set; }
        public string ActionType { get; set; } = string.Empty;
        public DateTime ActionAtUtc { get; set; }

        public QuantityMeasurementRecord? Measurement { get; set; }
    }
}
