using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using Wolverine.Weather.Domain.Interfaces;
using Wolverine.Weather.Infrastructure.Settings;

namespace Wolverine.Weather.Infrastructure.Repositories
{
    public class DatabaseConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly DatabaseConfigurationSection _databaseConfiguration;
        public DatabaseConnectionFactory(IOptions<DatabaseConfigurationSection> databaseConfiguration) //Constructor
        {
            _databaseConfiguration = databaseConfiguration.Value;
        }
        public int CommandTimeout => _databaseConfiguration.CommandTimeout;
        public int RetryAttempts => _databaseConfiguration.RetryAttempts;
        public IDbConnection GetWeatherDbConnection()
        {
            var connection = new SqlConnection(_databaseConfiguration.ConnectionStrings.WolverineDB);
            return connection;
        }
    }
}
