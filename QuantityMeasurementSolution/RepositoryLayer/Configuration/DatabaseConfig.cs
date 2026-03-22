namespace RepositoryLayer.Configuration
{
    public sealed class DatabaseConfig
    {
        public string ConnectionString { get; }

        public DatabaseConfig(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}