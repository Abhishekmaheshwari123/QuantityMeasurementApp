using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryLayer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "QuantityMeasurements",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Operation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstValue = table.Column<double>(type: "float", nullable: false),
                    FirstUnit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstMeasurementType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SecondValue = table.Column<double>(type: "float", nullable: true),
                    SecondUnit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SecondMeasurementType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ResultValue = table.Column<double>(type: "float", nullable: true),
                    ResultUnit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ResultMeasurementType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ResultText = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuantityMeasurements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuantityMeasurementHistory",
                schema: "dbo",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasurementId = table.Column<long>(type: "bigint", nullable: false),
                    ActionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ActionAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuantityMeasurementHistory", x => x.HistoryId);
                    table.ForeignKey(
                        name: "FK_QuantityMeasurementHistory_QuantityMeasurements_MeasurementId",
                        column: x => x.MeasurementId,
                        principalSchema: "dbo",
                        principalTable: "QuantityMeasurements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuantityMeasurementHistory_MeasurementId",
                schema: "dbo",
                table: "QuantityMeasurementHistory",
                column: "MeasurementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuantityMeasurementHistory",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "QuantityMeasurements",
                schema: "dbo");
        }
    }
}
