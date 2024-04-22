namespace Wolverine.Weather.Domain.ViewModels
{
    public class WeatherForecastViewModel
    {
        private const string celcuisSymbol = "°C";
        public DateTime DateAndTime { get; set; }
        public int TemperatureC { get; set; }
        public string TemperatureCDisplay => TemperatureC.ToString() + celcuisSymbol;
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        public string? TemperatureFDisplay => TemperatureF.ToString() + "°F";
        public string? Summary { get; set; }
        public Guid ExternalId { get; set; }
    }
}