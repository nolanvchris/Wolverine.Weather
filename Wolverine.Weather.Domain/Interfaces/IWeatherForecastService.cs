using Wolverine.Weather.Domain.Models;

namespace Wolverine.Weather.Domain.Interfaces
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecast> GetWeatherForecasts();
        WeatherForecast? GetWeatherForecast(int id); 
    }
}
