using Microsoft.EntityFrameworkCore;
using QuantityMeasurement.ModelLayer.Entities;

namespace QuantityMeasurement.RepositoryLayer.Data
{
    /// <summary>
    /// EF Core DbContext that maps quantity measurement and history tables.
    /// </summary>
    public sealed class QuantityMeasurementDbContext : DbContext
    {
        public QuantityMeasurementDbContext(DbContextOptions<QuantityMeasurementDbContext> options)
            : base(options)
        {
        }

        public DbSet<QuantityMeasurementRecord> QuantityMeasurements => Set<QuantityMeasurementRecord>();
        public DbSet<QuantityMeasurementHistoryRecord> QuantityMeasurementHistory => Set<QuantityMeasurementHistoryRecord>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuantityMeasurementRecord>(entity =>
            {
                entity.ToTable("QuantityMeasurements", "dbo");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Operation).HasMaxLength(50).IsRequired();
                entity.Property(x => x.FirstUnit).HasMaxLength(50).IsRequired();
                entity.Property(x => x.FirstMeasurementType).HasMaxLength(50).IsRequired();

                entity.Property(x => x.SecondUnit).HasMaxLength(50);
                entity.Property(x => x.SecondMeasurementType).HasMaxLength(50);

                entity.Property(x => x.ResultUnit).HasMaxLength(50);
                entity.Property(x => x.ResultMeasurementType).HasMaxLength(50);

                entity.Property(x => x.ResultText).HasMaxLength(255);
                entity.Property(x => x.ErrorMessage).HasMaxLength(500);

                entity.Property(x => x.CreatedAtUtc)
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("SYSUTCDATETIME()");
            });

            modelBuilder.Entity<QuantityMeasurementHistoryRecord>(entity =>
            {
                entity.ToTable("QuantityMeasurementHistory", "dbo");
                entity.HasKey(x => x.HistoryId);

                entity.Property(x => x.ActionType).HasMaxLength(50).IsRequired();
                entity.Property(x => x.ActionAtUtc)
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("SYSUTCDATETIME()");

                entity.HasOne(x => x.Measurement)
                    .WithMany(x => x.HistoryRecords)
                    .HasForeignKey(x => x.MeasurementId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
