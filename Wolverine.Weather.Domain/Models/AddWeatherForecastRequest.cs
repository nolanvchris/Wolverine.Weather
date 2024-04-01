namespace Wolverine.Weather.Domain.Models
{
    public class AddWeatherForecastRequest
    {
        public DateTime? Date { get; set; }
        public int? TemperatureC { get; set; }
        public string? Summary { get; set; }
    }
}
