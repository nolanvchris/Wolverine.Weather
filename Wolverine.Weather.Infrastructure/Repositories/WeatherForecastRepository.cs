using AutoMapper;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using Wolverine.Weather.Domain.Interfaces;
using Wolverine.Weather.Domain.Models;
using Wolverine.Weather.Infrastructure.Dtos;
using Wolverine.Weather.Infrastructure.Settings;

namespace Wolverine.Weather.Infrastructure.Repositories
{
    public class WeatherForecastRepository : IWeatherForecastRepository
    {
        private readonly IDatabaseConnectionFactory _databaseConnectionFactory; //This is how we instantiate an interface class.
        private readonly ILogger<WeatherForecastRepository> _logger;
        private readonly IMapper _mapper;
        private readonly DatabaseConfigurationSection _databaseConfiguration;

        public WeatherForecastRepository(
            IDatabaseConnectionFactory databaseConnectionFactory, 
            ILogger<WeatherForecastRepository> logger,
            IMapper mapper,
            IOptions<DatabaseConfigurationSection> databaseConfiguration) //IOptions is for values that are in the configuration file
        {
            _logger = logger;
            _databaseConnectionFactory = databaseConnectionFactory;
            _mapper = mapper;
            _databaseConfiguration = databaseConfiguration.Value;
        }

        public IEnumerable<WeatherForecast> GetWeatherForecasts()
        {
            using(var connection = _databaseConnectionFactory.GetWeatherDbConnection())
            {
                var result = connection.Query<WeatherForecast>("SELECT * FROM dbo.WeatherForecasts");
                return result;
            }
        }

        public WeatherForecast? GetWeatherForecast(int id)
        {
            using (var connection = _databaseConnectionFactory.GetWeatherDbConnection())
            {
                var result = connection.QueryFirstOrDefault<WeatherForecast>("SELECT TOP 1 * FROM dbo.WeatherForecasts WHERE WeatherForecastId = @id", new { id }); //You have to create a new anonymous object and pass the variable into it.
                return result;
            }
        }

        public async Task<WeatherForecast> AddWeatherForecast(AddWeatherForecastRequest request, CancellationToken cancellationToken)
        {
            try
            {
                string storedProcedure = _databaseConfiguration.StoredProcedureNames.WeatherForecastInsertStoredProcedureName;

                var parameters = new DynamicParameters();
                parameters.Add("@DateAndTime", request.Date, DbType.DateTime);
                parameters.Add("@TemperatureC", request.TemperatureC, DbType.Int32);
                parameters.Add("@Summary", request.Summary, DbType.String);

                using (var connection = _databaseConnectionFactory.GetWeatherDbConnection())
                {
                    var command = new CommandDefinition(
                        storedProcedure,
                        parameters,
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: _databaseConnectionFactory.CommandTimeout,
                        cancellationToken: cancellationToken);

                    var result = await connection.QueryFirstOrDefaultAsync<WeatherForecastDto>(command);
                    var fromSource = _mapper.Map<WeatherForecast>(result);
                    return fromSource;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error on {nameof(AddWeatherForecast)} exception message: {ex.Message}");
                throw;
            }
        }
    }
}
