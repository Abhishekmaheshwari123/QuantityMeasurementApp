CREATE DATABASE QuantityMeasurementDb;
GO

USE QuantityMeasurementDb;
GO

IF OBJECT_ID('dbo.QuantityMeasurementHistory', 'U') IS NOT NULL
    DROP TABLE dbo.QuantityMeasurementHistory;
GO

IF OBJECT_ID('dbo.QuantityMeasurements', 'U') IS NOT NULL
    DROP TABLE dbo.QuantityMeasurements;
GO

CREATE TABLE dbo.QuantityMeasurements
(
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,

    Operation NVARCHAR(50) NOT NULL,

    FirstValue FLOAT NOT NULL,
    FirstUnit NVARCHAR(50) NOT NULL,
    FirstMeasurementType NVARCHAR(50) NOT NULL,

    SecondValue FLOAT NULL,
    SecondUnit NVARCHAR(50) NULL,
    SecondMeasurementType NVARCHAR(50) NULL,

    ResultValue FLOAT NULL,
    ResultUnit NVARCHAR(50) NULL,
    ResultMeasurementType NVARCHAR(50) NULL,

    ResultText NVARCHAR(255) NULL,
    IsSuccess BIT NOT NULL DEFAULT(1),
    ErrorMessage NVARCHAR(500) NULL,

    CreatedAtUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

CREATE TABLE dbo.QuantityMeasurementHistory
(
    HistoryId BIGINT IDENTITY(1,1) PRIMARY KEY,
    MeasurementId BIGINT NOT NULL,
    ActionType NVARCHAR(50) NOT NULL,
    ActionAtUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT FK_QuantityMeasurementHistory_QuantityMeasurements
        FOREIGN KEY (MeasurementId)
        REFERENCES dbo.QuantityMeasurements(Id)
        ON DELETE CASCADE
);
GO

CREATE INDEX IX_QuantityMeasurements_Operation
ON dbo.QuantityMeasurements(Operation);
GO

CREATE INDEX IX_QuantityMeasurements_FirstMeasurementType
ON dbo.QuantityMeasurements(FirstMeasurementType);
GO

CREATE INDEX IX_QuantityMeasurements_CreatedAtUtc
ON dbo.QuantityMeasurements(CreatedAtUtc DESC);
GO
