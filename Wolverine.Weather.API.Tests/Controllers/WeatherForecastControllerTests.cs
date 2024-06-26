using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using Wolverine.Weather.API.Controllers;
using Wolverine.Weather.API.Profiles;
using Wolverine.Weather.Domain.Interfaces;
using Wolverine.Weather.Domain.Models;
using Wolverine.Weather.Domain.ViewModels;

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
            const string summary = "Tests"; //using constant to make these tamper proof.
            const int temperatureC = 32;
            
            var request = new AddWeatherForecastRequest
            {
                Summary = summary,
                DateAndTime = DateTime.UtcNow,
                TemperatureC = temperatureC,
            };

            _mockWeatherForecastService.Setup(x => x.AddWeatherForecast(
                It.IsAny<AddWeatherForecastRequest>(), //It.IsAny any parameter
                It.IsAny<CancellationToken>() 
                )).ReturnsAsync(new WeatherForecast //Forces this model to be returned because we don't need to go further than the service layer
                { 
                    DateAndTime = request.DateAndTime.Value, 
                    Summary = request.Summary, 
                    TemperatureC = request.TemperatureC.Value 
                });

            //Act
            var weatherForecastController = ClassUnderTest();
            var response = await weatherForecastController.Post(request, CancellationToken.None);
            
            //Assert
            using(new AssertionScope())
            {
                response.Should().NotBeNull();

                var test = response.Result as OkObjectResult;
                test!.StatusCode.HasValue.Should().BeTrue(); 
                test.StatusCode!.Value.Should().Be((int)HttpStatusCode.OK); 
                test.Value.Should().BeOfType<WeatherForecastViewModel>();
                test.Value.Should().BeEquivalentTo(new WeatherForecastViewModel
                {
                    Summary = request.Summary,
                    DateAndTime = request.DateAndTime.Value,
                    TemperatureC = request.TemperatureC.Value,
                });
                var viewModel = test.Value as WeatherForecastViewModel;
                viewModel!.TemperatureCDisplay.Should().Be(request.TemperatureC.ToString() + "�C");
                viewModel!.TemperatureF.Should().Be(32 + (int)(request.TemperatureC / 0.5556));
                viewModel!.TemperatureFDisplay.Should().Be((32 + (int)(request.TemperatureC / 0.5556)).ToString() + "�F");
            }
        }
        #region BadRequestTests
        [TestMethod]
        public async Task Post_DateTimeNoValue_ReturnsBadRequest()
        {
            //Arrange
            const string summary = "Tests"; //using constant to make these temper proof.
            const int temperatureC = 32;

            var request = new AddWeatherForecastRequest
            {
                Summary = summary,
                DateAndTime = null,
                TemperatureC = temperatureC,
            };

            //Act
            var weatherForecastController = ClassUnderTest();
            var response = await weatherForecastController.Post(request, CancellationToken.None);

            //Assert
            using (new AssertionScope()) //Assertion Scope will allow more than one assertion to be tested at a time.
            {
                response.Should().NotBeNull(); //Should be null? 

                var test = response.Result as BadRequestObjectResult;
                test!.StatusCode.HasValue.Should().BeTrue(); // should have a value
                test.StatusCode!.Value.Should().Be((int)HttpStatusCode.BadRequest); // should be indicating an issue
            }
        }
        [TestMethod]
        public async Task Post_TemperatureCNoValue_ReturnsBadRequest()
        {
            //Arrange
            const string summary = "Tests"; //using constant to make these temper proof.

            var request = new AddWeatherForecastRequest
            {
                Summary = summary,
                DateAndTime = DateTime.UtcNow,
                TemperatureC = null,
            };

            //Act
            var weatherForecastController = ClassUnderTest();
            var response = await weatherForecastController.Post(request, CancellationToken.None);

            //Assert
            using (new AssertionScope())
            {
                response.Should().NotBeNull(); //Should be null? 

                var test = response.Result as BadRequestObjectResult;
                test!.StatusCode.HasValue.Should().BeTrue(); // should have a value
                test.StatusCode!.Value.Should().Be((int)HttpStatusCode.BadRequest); // should be indicating an issue
            }
        }
        [TestMethod]
        public async Task Post_SummaryNoValue_ReturnsBadRequest()
        {
            //Arrange
            const int temperatureC = 32;

            var request = new AddWeatherForecastRequest
            {
                Summary = null,
                DateAndTime = DateTime.UtcNow,
                TemperatureC = temperatureC,
            };

            //Act
            var weatherForecastController = ClassUnderTest();
            var response = await weatherForecastController.Post(request, CancellationToken.None);

            //Assert
            using (new AssertionScope())
            {
                response.Should().NotBeNull(); 

                var test = response.Result as BadRequestObjectResult;
                test!.StatusCode.HasValue.Should().BeTrue(); 
                test.StatusCode!.Value.Should().Be((int)HttpStatusCode.BadRequest); 
            }
        }

        [TestMethod]
        public async Task Post_RequestNoValues_ReturnsBadRequest()
        {
            //Arrange
            var request = new AddWeatherForecastRequest
            {
                Summary = null,
                DateAndTime = null,
                TemperatureC = null,
            };

            //Act
            var weatherForecastController = ClassUnderTest();
            var response = await weatherForecastController.Post(request, CancellationToken.None);

            //Assert
            using (new AssertionScope())
            {
                response.Should().NotBeNull();

                var test = response.Result as BadRequestObjectResult;
                test!.StatusCode.HasValue.Should().BeTrue();
                test.StatusCode!.Value.Should().Be((int)HttpStatusCode.BadRequest);
            }
        }

        #endregion
    }
}