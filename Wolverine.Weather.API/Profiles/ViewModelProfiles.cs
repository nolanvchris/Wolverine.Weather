using AutoMapper;
using Wolverine.Weather.Domain.Models;
using Wolverine.Weather.Domain.ViewModels;

namespace Wolverine.Weather.API.Profiles
{
    public class ViewModelProfiles : Profile
    {
        public ViewModelProfiles()
        {
            CreateMap<WeatherForecast, WeatherForecastViewModel>().ReverseMap();
        }
    }
}
