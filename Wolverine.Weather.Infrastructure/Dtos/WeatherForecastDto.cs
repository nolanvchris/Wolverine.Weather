﻿namespace Wolverine.Weather.Infrastructure.Dtos
{
    public class WeatherForecastDto
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public string? Summary { get; set; }
    }
}
