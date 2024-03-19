using Dapper;
using System.Data.SqlClient;
using Wolverine.Weather.Domain.Interfaces;
using Wolverine.Weather.Domain.Models;

namespace Wolverine.Weather.Infrastructure.Repositories
{
    public class WeatherForecastRepository : IWeatherForecastRepository
    {
        private readonly IDatabaseConnectionFactory _databaseConnectionFactory; //This is how we instantiate an interface class.
        public WeatherForecastRepository(IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _databaseConnectionFactory = databaseConnectionFactory;
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
    }
}
