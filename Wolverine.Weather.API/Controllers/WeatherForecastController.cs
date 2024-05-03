using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wolverine.Weather.Domain.Exceptions;
using Wolverine.Weather.Domain.Interfaces;
using Wolverine.Weather.Domain.Models;
using Wolverine.Weather.Domain.ViewModels;

namespace Wolverine.Weather.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherForecastService _weatherForecastService;
        private readonly IMapper _mapper;
        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IWeatherForecastService weatherForecastService,
            IMapper mapper
            )
        {
            _weatherForecastService = weatherForecastService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _weatherForecastService.GetWeatherForecasts(cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("{ExternalId}")]
        public async Task<ActionResult<WeatherForecast>> Get(Guid ExternalId, CancellationToken cancellationToken)
        {
            var result = await _weatherForecastService.GetWeatherForecast(ExternalId, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Add WeatherForecast
        /// </summary>
        /// <remarks>DateTime must always be unique, no repeat times allowed.</remarks>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<WeatherForecastViewModel>> Post(AddWeatherForecastRequest request, CancellationToken cancellationToken)
        {
            if(request is null || !request.DateAndTime.HasValue || !request.TemperatureC.HasValue || string.IsNullOrWhiteSpace(request.Summary))
            {
                return BadRequest("Missing field");
            }
            try
            {
                var result = await _weatherForecastService.AddWeatherForecast(request, cancellationToken);
                var viewModel = _mapper.Map<WeatherForecastViewModel>(result);
                return Ok(viewModel);
            }
            catch(InsertWeatherForecastValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}