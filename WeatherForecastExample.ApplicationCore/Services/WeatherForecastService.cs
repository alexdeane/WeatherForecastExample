using WeatherForecastExample.ApplicationCore.Abstraction;
using WeatherForecastExample.ApplicationCore.Clients;
using WeatherForecastExample.ApplicationCore.Models;

namespace WeatherForecastExample.ApplicationCore.Services;

public interface IWeatherForecastService
{
    Task<ServiceResult<WeatherForecastResult>> GetForecasts(string locationName, CancellationToken cancellationToken);
}

public class WeatherForecastService : IWeatherForecastService
{
    private readonly IGeoCodingClient _geoCodingClient;
    private readonly IWeatherClient _weatherClient;
    private readonly IOpenMeteoMappingService _mappingService;

    public WeatherForecastService(IGeoCodingClient geoCodingClient, IWeatherClient weatherClient,
        IOpenMeteoMappingService mappingService)
    {
        _geoCodingClient = geoCodingClient;
        _weatherClient = weatherClient;
        _mappingService = mappingService;
    }

    public async Task<ServiceResult<WeatherForecastResult>> GetForecasts(string locationName, CancellationToken cancellationToken)
    {
        var locationResult = await _geoCodingClient.GetLocation(locationName, cancellationToken);

        var location = locationResult.Results!.First();

        var weatherData = await _weatherClient.GetWeatherForecasts(
            latitude: location.Latitude,
            longitude: location.Longitude,
            cancellationToken: cancellationToken
        );

        var result = _mappingService.Map(weatherData, location);

        return new ServiceResult<WeatherForecastResult>(result);
    }
}