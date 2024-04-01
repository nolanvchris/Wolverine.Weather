namespace Wolverine.Weather.Domain.Models
{
    /// <summary>
    /// WeatherForecast Model to only be used internally, passing data from the repository up to the controller level.
    /// </summary>
    public class WeatherForecast
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        public string? Summary { get; set; }
    }
}