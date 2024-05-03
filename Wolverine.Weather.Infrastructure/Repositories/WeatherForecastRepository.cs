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
        private readonly IDatabaseConnectionFactory _databaseConnectionFactory;
        private readonly ILogger<WeatherForecastRepository> _logger;
        private readonly IMapper _mapper;
        private readonly DatabaseConfigurationSection _databaseConfiguration;

        public WeatherForecastRepository(
            IDatabaseConnectionFactory databaseConnectionFactory, 
            ILogger<WeatherForecastRepository> logger,
            IMapper mapper,
            IOptions<DatabaseConfigurationSection> databaseConfiguration)
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
                string storedProcedure = _databaseConfiguration.StoredProcedureNames.WeatherForecastGetAll;

                using (var connection = _databaseConnectionFactory.GetWeatherDbConnection())
                {
                    var command = new CommandDefinition(
                        storedProcedure,
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: _databaseConnectionFactory.CommandTimeout,
                        cancellationToken: cancellationToken);

                    var result = await connection.QueryAsync<WeatherForecastDto>(command);
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

        public async Task<bool> IsWeatherForecastDateAlreadyUsed(DateTime date, CancellationToken cancellationToken)
        {
            try
            {
                string storedProcedure = _databaseConfiguration.StoredProcedureNames.WeatherForecastGetByDate; //TODO 4/30: Create New stored procedure in DB. Return a boolean from DB 1 or 0. Ask Chat GPT for script to see if a certain piece of data exists.
                var parameters = new DynamicParameters();

                parameters.Add("@pDateAndTime", date, DbType.DateTime);
                using (var connection = _databaseConnectionFactory.GetWeatherDbConnection())
                {
                    var command = new CommandDefinition(
                        storedProcedure,
                        parameters,
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: _databaseConnectionFactory.CommandTimeout,
                        cancellationToken: cancellationToken);

                    return await connection.QuerySingleAsync<bool>(command);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error on {nameof(IsWeatherForecastDateAlreadyUsed)} exception message: {ex.Message}");
                throw;
            }
        }

        public async Task<WeatherForecast> GetWeatherForecast(Guid ExternalId, CancellationToken cancellationToken)
        {
            try
            {
                string storedProcedure = _databaseConfiguration.StoredProcedureNames.WeatherForecastGetById;

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

                    var result = await connection.QuerySingleOrDefaultAsync<WeatherForecastDto>(command);
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
                string storedProcedure = _databaseConfiguration.StoredProcedureNames.WeatherForecastInsert;

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
            catch (ArgumentNullException ex) //More specific to general with catch clauses.
            {
                _logger.LogError($"You missed a parameter for this method");
                throw;
            }
            catch (SqlException ex)
            {
                _logger.LogError($"Error on {nameof(AddWeatherForecast)} SQL exception message: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error on {nameof(AddWeatherForecast)} exception message: {ex.Message}");
                throw;
            }
        }
    }
}
