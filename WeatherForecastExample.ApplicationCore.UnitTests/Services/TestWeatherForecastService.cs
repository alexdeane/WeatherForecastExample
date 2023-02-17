using Moq;
using WeatherForecastExample.ApplicationCore.Clients;
using WeatherForecastExample.ApplicationCore.Clients.Contracts;
using WeatherForecastExample.ApplicationCore.Models;
using WeatherForecastExample.ApplicationCore.Services;

namespace WeatherForecastExample.ApplicationCore.UnitTests.Services;

public class TestWeatherForecastService
{
    private readonly WeatherForecastService _weatherForecastService;
    private readonly Mock<IGeoCodingClient> _mockGeoCodingClient;
    private readonly Mock<IWeatherClient> _mockWeatherClient;
    private readonly Mock<IOpenMeteoMappingService> _mockMappingService;

    public TestWeatherForecastService()
    {
        _mockMappingService = new Mock<IOpenMeteoMappingService>();
        _mockWeatherClient = new Mock<IWeatherClient>();
        _mockGeoCodingClient = new Mock<IGeoCodingClient>();

        _weatherForecastService = new WeatherForecastService(_mockGeoCodingClient.Object, _mockWeatherClient.Object,
            _mockMappingService.Object);
    }

    [Fact]
    public async Task HappyPath()
    {
        var search = "search";
        var weatherResult = new WeatherForecastResult();

        // Arrange
        // Set up the geo coding client interface to return a mocked result
        _mockGeoCodingClient
            .Setup(x => x.GetLocation(search, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OpenMeteoLocationResponse
            {
                Results = new OpenMeteoLocationResult[]
                {
                    new()
                    {
                        Latitude = 4,
                        Longitude = 5
                    }
                }
            });

        // Set up the weather clin client interface to return a mocked result
        _mockWeatherClient.Setup(x => x.GetWeatherForecasts(4, 5, CancellationToken.None))
            .ReturnsAsync(new OpenMeteoWeatherResponse());

        _mockMappingService.Setup(x =>
                x.Map(It.IsAny<OpenMeteoWeatherResponse>(), It.IsAny<OpenMeteoLocationResult>()))
            .Returns(weatherResult);

        // Act
        var serviceResult = await _weatherForecastService.GetForecasts(search, CancellationToken.None);

        // Assert
        Assert.Null(serviceResult.UserSafeError);
        Assert.NotNull(serviceResult.Result);

        // Verify
        _mockGeoCodingClient.Verify(x => x.GetLocation(search, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ErrorPath_GeoCodingClient_Throws()
    {
        const string search = "search";

        // Arrange
        _mockGeoCodingClient
            .Setup(x => x.GetLocation(search, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ClientException("foo"));

        _mockWeatherClient.Setup(x => x.GetWeatherForecasts(4, 5, CancellationToken.None));

        _mockMappingService.Setup(x =>
            x.Map(It.IsAny<OpenMeteoWeatherResponse>(), It.IsAny<OpenMeteoLocationResult>()));

        // Act
        var serviceResult = await _weatherForecastService.GetForecasts(search, CancellationToken.None);

        // Assert
        Assert.NotNull(serviceResult.UserSafeError);
        Assert.Null(serviceResult.Result);
        Assert.Equal("foo", serviceResult.UserSafeError);

        // Verify
        _mockGeoCodingClient.Verify(x => x.GetLocation(search, It.IsAny<CancellationToken>()), Times.Once);
        _mockWeatherClient.Verify(x => x.GetWeatherForecasts(4, 5, CancellationToken.None), Times.Never);
        _mockMappingService.Verify(x =>
            x.Map(It.IsAny<OpenMeteoWeatherResponse>(), It.IsAny<OpenMeteoLocationResult>()), Times.Never);
    }
}