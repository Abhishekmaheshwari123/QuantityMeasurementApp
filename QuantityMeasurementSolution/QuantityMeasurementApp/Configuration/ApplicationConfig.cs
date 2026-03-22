namespace QuantityMeasurementApp.Configuration
{
    public sealed class ApplicationConfig
    {
        public bool UseDatabaseRepository { get; }
        public string ConnectionString { get; }

        public ApplicationConfig(bool useDatabaseRepository, string connectionString)
        {
            UseDatabaseRepository = useDatabaseRepository;
            ConnectionString = connectionString;
        }
    }
}