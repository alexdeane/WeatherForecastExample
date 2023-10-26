using Moq;
using WeatherForecastExample.ApplicationCore.Clients;
using WeatherForecastExample.ApplicationCore.Clients.Contracts;
using WeatherForecastExample.ApplicationCore.Models;
using WeatherForecastExample.ApplicationCore.Services;

namespace WeatherForecastExample.ApplicationCore.UnitTests.Services;

/// <summary>
/// Class containing unit tests. Generally you would make one class for each "unit"
/// of the application that you are testing. In this case, we are testing the <see cref="WeatherForecastService"/>
/// </summary>
public class TestWeatherForecastService
{
    private readonly WeatherForecastService _weatherForecastService;
    private readonly Mock<IGeoCodingClient> _mockGeoCodingClient;
    private readonly Mock<IWeatherClient> _mockWeatherClient;
    private readonly Mock<IOpenMeteoMappingService> _mockMappingService;

    /// <summary>
    /// Use the constructor to initialize things
    /// </summary>
    public TestWeatherForecastService()
    {
        // Set up mocked services. If we were to pass REAL services to the
        // instance of WeatherForecastService that we are testing, we'd be testing
        // multiple units at once. Instead, we create "mocked" versions of them.
        
        // The Moq library just provides a convenient API for something that you
        // could do yourself - stubbing interfaces (or abstracts/virtuals) to return
        // predetermined values in place of a real implementation.
        _mockMappingService = new Mock<IOpenMeteoMappingService>();
        _mockWeatherClient = new Mock<IWeatherClient>();
        _mockGeoCodingClient = new Mock<IGeoCodingClient>();

        // Note that the only 'real' class we instantiate is the service under test
        _weatherForecastService = new WeatherForecastService(
            geoCodingClient: _mockGeoCodingClient.Object,
            weatherClient: _mockWeatherClient.Object,
            mappingService: _mockMappingService.Object
        );
    }

    // Each 'Fact' is a unit test which takes no parameters to run.
    // A 'Theory' (not exemplified here) is a unit test which takes parameters to run, and will run
    // more than once for each provided parameter set.
    [Fact]
    public async Task HappyPath()
    {
        // Set up constants and variables for inputs/outputs
        const string search = "search";
        const int latitude = 4;
        const int longitude = 5;

        var clientResponse = new OpenMeteoWeatherResponse();
        var weatherResult = new WeatherForecastResult();
        var locationResult = new OpenMeteoLocationResult
        {
            Latitude = latitude,
            Longitude = longitude
        };

        // Arrange
        // Set up the geo coding client interface to return a mocked result
        _mockGeoCodingClient
            .Setup(x => x.GetLocation(search, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OpenMeteoLocationResponse
            {
                Results = new[]
                {
                    locationResult
                }
            });

        // Set up the weather client interface to return a mocked result
        _mockWeatherClient.Setup(x => x.GetWeatherForecasts(4, 5, CancellationToken.None))
            .ReturnsAsync(clientResponse);

        _mockMappingService.Setup(x =>
                x.Map(It.IsAny<OpenMeteoWeatherResponse>(), It.IsAny<OpenMeteoLocationResult>()))
            .Returns(weatherResult);

        // Act - invoke the service to return a result
        var serviceResult = await _weatherForecastService.GetForecasts(search, CancellationToken.None);

        // Assert - Check that the result matches what we expect
        Assert.IsType<WeatherForecastResult>(serviceResult.Value);
        Assert.Equal(weatherResult, serviceResult.Value);

        // Verify - Ensure that the service invoked the dependencies that we expect
        _mockGeoCodingClient.Verify(x => x.GetLocation(search, It.IsAny<CancellationToken>()), Times.Once);
        _mockWeatherClient.Verify(x => x.GetWeatherForecasts(latitude, longitude, It.IsAny<CancellationToken>()), Times.Once);
        _mockMappingService.Verify(x => x.Map(clientResponse, locationResult), Times.Once);
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
        Assert.IsType<WeatherForecastError>(serviceResult.Value);

        var error = (WeatherForecastError) serviceResult.Value;

        Assert.NotNull(error.UserSafeErrorMessage);
        Assert.Equal("foo", error.UserSafeErrorMessage);

        // Verify
        _mockGeoCodingClient.Verify(x => x.GetLocation(search, It.IsAny<CancellationToken>()), Times.Once);
        _mockWeatherClient.Verify(x => x.GetWeatherForecasts(4, 5, CancellationToken.None), Times.Never);
        _mockMappingService.Verify(x =>
            x.Map(It.IsAny<OpenMeteoWeatherResponse>(), It.IsAny<OpenMeteoLocationResult>()), Times.Never);
    }
}