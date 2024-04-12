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
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetAll(CancellationToken cancellationToken) //HOMEWORK 4.2.2024: Do this the correct way 
        {
            var result = await _weatherForecastService.GetWeatherForecasts(cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<WeatherForecast>> Get(int id, CancellationToken cancellationToken)
        {
            if(id <= 0)
            {
                return BadRequest($"Invalid Id:{id}");
            }
            var result = await _weatherForecastService.GetWeatherForecast(id, cancellationToken);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<WeatherForecastViewModel>> Post(AddWeatherForecastRequest request, CancellationToken cancellationToken)
        {
            if(request is null || !request.DateAndTime.HasValue || !request.TemperatureC.HasValue || string.IsNullOrWhiteSpace(request.Summary))
            {
                return BadRequest("Hey dummy you forgot a field");
            }
            var result = await _weatherForecastService.AddWeatherForecast(request, cancellationToken);
            var viewModel = _mapper.Map<WeatherForecastViewModel>(result); //HomeWork 4/5/2024: Date and TemperatureF are not getting values from the database, perhaps something wrong with mapping. Fix this
            return Ok(viewModel);
        }
    }
}