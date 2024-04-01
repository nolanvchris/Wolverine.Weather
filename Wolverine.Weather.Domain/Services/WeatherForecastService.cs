using Wolverine.Weather.Domain.Interfaces;
using Wolverine.Weather.Domain.Models;

namespace Wolverine.Weather.Domain.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly IWeatherForecastRepository _weatherForecastRepository; 
        public WeatherForecastService(IWeatherForecastRepository weatherForecastRepository) //Constructor
        {
            _weatherForecastRepository = weatherForecastRepository;
        }

        public async Task<WeatherForecast> GetWeatherForecast(int id, CancellationToken cancellationToken)
        {
            return await _weatherForecastRepository.GetWeatherForecast(id, cancellationToken);
        }

        public IEnumerable<WeatherForecast> GetWeatherForecasts()
        {
            return _weatherForecastRepository.GetWeatherForecasts();
        }

        public async Task<WeatherForecast> AddWeatherForecast(AddWeatherForecastRequest request, CancellationToken cancellationToken)
        {
            return await _weatherForecastRepository.AddWeatherForecast(request, cancellationToken);
        }
    }
}
