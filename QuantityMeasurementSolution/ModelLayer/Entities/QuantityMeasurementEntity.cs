using System;

namespace ModelLayer.Entities
{
    [Serializable]
    public sealed class QuantityMeasurementEntity
    {
        public DateTime TimestampUtc { get; }
        public string Operation { get; }

        public double FirstValue { get; }
        public string FirstUnit { get; }
        public string FirstMeasurementType { get; }

        public double? SecondValue { get; }
        public string? SecondUnit { get; }
        public string? SecondMeasurementType { get; }

        public double? ResultValue { get; }
        public string? ResultUnit { get; }
        public string? ResultMeasurementType { get; }

        public string? ResultText { get; }
        public bool IsSuccess { get; }
        public string? ErrorMessage { get; }

        public QuantityMeasurementEntity(
            string operation,
            double firstValue,
            string firstUnit,
            string firstMeasurementType,
            double? secondValue = null,
            string? secondUnit = null,
            string? secondMeasurementType = null,
            double? resultValue = null,
            string? resultUnit = null,
            string? resultMeasurementType = null,
            string? resultText = null,
            bool isSuccess = true,
            string? errorMessage = null)
        {
            TimestampUtc = DateTime.UtcNow;
            Operation = operation ?? throw new ArgumentNullException(nameof(operation));
            FirstValue = firstValue;
            FirstUnit = firstUnit ?? throw new ArgumentNullException(nameof(firstUnit));
            FirstMeasurementType = firstMeasurementType ?? throw new ArgumentNullException(nameof(firstMeasurementType));
            SecondValue = secondValue;
            SecondUnit = secondUnit;
            SecondMeasurementType = secondMeasurementType;
            ResultValue = resultValue;
            ResultUnit = resultUnit;
            ResultMeasurementType = resultMeasurementType;
            ResultText = resultText;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }
    }
}