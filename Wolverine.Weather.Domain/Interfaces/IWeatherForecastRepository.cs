using Wolverine.Weather.Domain.Models;

namespace Wolverine.Weather.Domain.Interfaces
{
    public interface IWeatherForecastRepository
    {
        public IEnumerable<WeatherForecast> GetWeatherForecasts();
    }
}
