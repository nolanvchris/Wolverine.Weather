namespace Wolverine.Weather.Infrastructure.Dtos
{
    //These properties must match column names in Database
    public class WeatherForecastDto
    {
        public DateTime DateAndTime { get; set; }
        public int TemperatureC { get; set; }
        public string? Summary { get; set; }
        public Guid ExternalId { get; set; } 
    }
}
