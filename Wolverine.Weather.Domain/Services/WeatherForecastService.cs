using Wolverine.Weather.Domain.Interfaces;
using Wolverine.Weather.Domain.Models;

namespace Wolverine.Weather.Domain.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly IWeatherForecastRepository _weatherForecastRepository; 
        public WeatherForecastService(IWeatherForecastRepository weatherForecastRepository)
        {
            _weatherForecastRepository = weatherForecastRepository;
        }
        public IEnumerable<WeatherForecast> GetWeatherForecasts()
        {
            return _weatherForecastRepository.GetWeatherForecasts();
        }
        //Method for returning a weather forecast by ID:

    }
}
