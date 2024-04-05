namespace Wolverine.Weather.Domain.ViewModels
{
    public class WeatherForecastViewModel
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public string TemperatureCDisplay => TemperatureC.ToString() + "°";
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        public string? TemperatureFDisplay { get; set; }
        public string? Summary { get; set; }
    }
}
