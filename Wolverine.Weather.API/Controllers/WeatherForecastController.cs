using Microsoft.AspNetCore.Mvc;
using Wolverine.Weather.Domain.Interfaces;
using Wolverine.Weather.Domain.Models;
using Wolverine.Weather.Domain.ViewModels; //Thinking about using this later...

namespace Wolverine.Weather.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger; 
        private readonly IWeatherForecastService _weatherForecastService;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService weatherForecastService)
        {
            _logger = logger;
            _weatherForecastService = weatherForecastService;
        }

        [HttpGet] //The get attribute tag does not need a name Ex. [HttpGet(Name = GetWeatherForecast)]. C# will name it after the controller by truncating off the "*Controller" part
        public IEnumerable<WeatherForecast> GetAll()
        {
            var result = _weatherForecastService.GetWeatherForecasts();
            return result;
        }
        [HttpGet]
        [Route("{id}")]
        //Homework return 1 specific forecast.
        public WeatherForecast Get(int id)
        {
            var result = _weatherForecastService.GetWeatherForecast(id);
            return result;
        }
    }
}