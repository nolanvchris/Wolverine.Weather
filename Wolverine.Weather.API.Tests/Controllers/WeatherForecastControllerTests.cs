using AutoMapper;
using Microsoft.Extensions.Logging;
using Wolverine.Weather.API.Controllers;
using Wolverine.Weather.API.Profiles;
using Wolverine.Weather.Domain.Interfaces;
using Wolverine.Weather.Domain.Models;

namespace Wolverine.Weather.API.Tests.Controllers
{
    [TestClass]
    public class WeatherForecastControllerTests
    {
        private readonly Mock<IWeatherForecastService> _mockWeatherForecastService = new Mock<IWeatherForecastService>();
        private readonly Mock<ILogger<WeatherForecastController>> _mockLogger = new Mock<ILogger<WeatherForecastController>>();

        public WeatherForecastController ClassUnderTest()
        {
            var mapper = new MapperConfiguration(x => x.AddProfile<ViewModelProfiles>()).CreateMapper();
            return new WeatherForecastController(_mockLogger.Object, _mockWeatherForecastService.Object, mapper);
        }

        [TestMethod]
        public async Task Post_HappyPath() //successful run through post. no errors
        {
            //Arrange
            var request = new AddWeatherForecastRequest
            {
                Summary = "Tests",
                Date = DateTime.UtcNow,
                TemperatureC = 32,
            };

            _mockWeatherForecastService.Setup(x => x.AddWeatherForecast(
                It.IsAny<AddWeatherForecastRequest>(), //It.IsAny any parameter
                It.IsAny<CancellationToken>() 
                )).ReturnsAsync(new WeatherForecast //Forces this to be returned
                { 
                    Date = request.Date.Value, 
                    Summary = request.Summary, 
                    TemperatureC = request.TemperatureC.Value 
                });

            //Act
            var weatherForecastController = ClassUnderTest();
            var result = await weatherForecastController.Post(request, CancellationToken.None);

            //Assert
            using(new AssertionScope()) //Assertion Scope will
            {
                Assert.IsNotNull(result);
                //Assert.AreEqual(request.Summary, result.Result.StatusCode);
            }
            
        }
    }
}