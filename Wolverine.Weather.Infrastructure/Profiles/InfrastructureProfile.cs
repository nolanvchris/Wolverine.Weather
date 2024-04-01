using AutoMapper;
using Wolverine.Weather.Domain.Models;
using Wolverine.Weather.Infrastructure.Dtos;

namespace Wolverine.Weather.Infrastructure.Profiles
{
    public class InfrastructureProfile : Profile
    {
        public InfrastructureProfile() 
        {
            CreateMap<WeatherForecastDto, WeatherForecast>().ReverseMap();
        }
    }
}
