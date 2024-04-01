using AutoMapper;
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
        private readonly IMapper _mapper;
        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IWeatherForecastService weatherForecastService,
            IMapper mapper
            )
        {
            _logger = logger;
            _weatherForecastService = weatherForecastService;
            _mapper = mapper;
        }
        [HttpGet] //The get attribute tag does not need a name Ex. [HttpGet(Name = GetWeatherForecast)]. C# will name it after the controller by truncating off the "*Controller" part
        public IEnumerable<WeatherForecast> GetAll()
        {
            var result = _weatherForecastService.GetWeatherForecasts();
            return result;
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<WeatherForecast> Get(int id, CancellationToken cancellationToken) //Homework: Fix this
        {
            var result = await _weatherForecastService.GetWeatherForecast(id, cancellationToken);
            return result;
        }
        [HttpPost]
        public async Task<ActionResult> Post(AddWeatherForecastRequest request, CancellationToken cancellationToken)
        {
            if(request is null || !request.Date.HasValue || !request.TemperatureC.HasValue || string.IsNullOrWhiteSpace(request.Summary))
            {
                return BadRequest("Hey dummy you forgot a field");
            }
            var result = await _weatherForecastService.AddWeatherForecast(request, cancellationToken);
            var viewModel = _mapper.Map<WeatherForecastViewModel>(result); 
            return Ok(result);
        }
        
    }
}