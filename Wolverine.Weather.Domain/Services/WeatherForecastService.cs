using Microsoft.Extensions.Logging;
using Wolverine.Weather.Domain.Exceptions;
using Wolverine.Weather.Domain.Interfaces;
using Wolverine.Weather.Domain.Models;

namespace Wolverine.Weather.Domain.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly IWeatherForecastRepository _weatherForecastRepository;
        private readonly ILogger<WeatherForecastService> _logger;
        public WeatherForecastService(
            IWeatherForecastRepository weatherForecastRepository,
            ILogger<WeatherForecastService> logger) //Constructor
        {

            _weatherForecastRepository = weatherForecastRepository;
            _logger = logger;
        }

        public async Task<WeatherForecast> GetWeatherForecast(Guid ExternalId, CancellationToken cancellationToken)
        {
            return await _weatherForecastRepository.GetWeatherForecast(ExternalId, cancellationToken);
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecasts(CancellationToken cancellationToken)
        {
            return await _weatherForecastRepository.GetWeatherForecasts(cancellationToken);
        }

        public async Task<WeatherForecast> AddWeatherForecast(AddWeatherForecastRequest request, CancellationToken cancellationToken)
        {
            var isDateUsed = await _weatherForecastRepository.IsWeatherForecastDateAlreadyUsed(request.DateAndTime!.Value, cancellationToken);
            if (isDateUsed)
            {
                throw new InsertWeatherForecastValidationException($"The date {request.DateAndTime!.Value} is already used");
            }
            return await _weatherForecastRepository.AddWeatherForecast(request, cancellationToken);
        }
    }
}