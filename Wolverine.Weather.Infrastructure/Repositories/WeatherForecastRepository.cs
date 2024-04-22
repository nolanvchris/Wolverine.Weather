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
        private readonly IDatabaseConnectionFactory _databaseConnectionFactory; //These are the dependencies, instead of creating them in the class we are "injecting" them in
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

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecasts(CancellationToken cancellationToken)
        {
            try
            {
                string storedProcedure = _databaseConfiguration.StoredProcedureNames.WeatherForecastGetAllStoredProcedureName;

                using (var connection = _databaseConnectionFactory.GetWeatherDbConnection())
                {
                    var command = new CommandDefinition(
                        storedProcedure,
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: _databaseConnectionFactory.CommandTimeout,
                        cancellationToken: cancellationToken);

                    var result = await connection.QueryAsync<WeatherForecastDto>(command);//Single or default finds only one thing or throws exception.
                    var fromSource = _mapper.Map<IEnumerable<WeatherForecast>>(result);
                    return fromSource;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error on {nameof(GetWeatherForecasts)} exception message: {ex.Message}");
                throw;
            }
        }

        public async Task<WeatherForecast> GetWeatherForecast(Guid ExternalId, CancellationToken cancellationToken)
        {
            try
            {
                string storedProcedure = _databaseConfiguration.StoredProcedureNames.WeatherForecastGetByIdStoredProcedureName;

                var parameters = new DynamicParameters();

                parameters.Add("@pExternalId", ExternalId, DbType.Guid);

                using (var connection = _databaseConnectionFactory.GetWeatherDbConnection())
                {
                    var command = new CommandDefinition(
                        storedProcedure,
                        parameters,
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: _databaseConnectionFactory.CommandTimeout,
                        cancellationToken: cancellationToken);

                    var result = await connection.QuerySingleOrDefaultAsync<WeatherForecastDto>(command);//Single or default finds only one thing or throws exception.
                    var fromSource = _mapper.Map<WeatherForecast>(result);
                    return fromSource;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error on {nameof(GetWeatherForecast)} exception message: {ex.Message}");
                throw;
            }
        }

        public async Task<WeatherForecast> AddWeatherForecast(AddWeatherForecastRequest request, CancellationToken cancellationToken)
        {
            try
            {
                string storedProcedure = _databaseConfiguration.StoredProcedureNames.WeatherForecastInsertStoredProcedureName;

                var parameters = new DynamicParameters();
                parameters.Add("@pDateAndTime", request.DateAndTime, DbType.DateTime);
                parameters.Add("@pTemperatureC", request.TemperatureC, DbType.Int32);
                parameters.Add("@pSummary", request.Summary, DbType.String);

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
