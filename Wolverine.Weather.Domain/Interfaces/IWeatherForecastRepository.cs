﻿using Wolverine.Weather.Domain.Models;

namespace Wolverine.Weather.Domain.Interfaces
{
    public interface IWeatherForecastRepository
    {
        Task<IEnumerable<WeatherForecast>> GetWeatherForecasts(CancellationToken cancellationToken);
        Task<WeatherForecast> GetWeatherForecast(Guid ExternalId, CancellationToken cancellationToken);
        Task<WeatherForecast> AddWeatherForecast(AddWeatherForecastRequest request, CancellationToken cancellationToken);
        Task<bool> IsWeatherForecastDateAlreadyUsed(DateTime date, CancellationToken cancellationToken);
    }
}
