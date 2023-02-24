using OneOf;
using WeatherForecastExample.ApplicationCore.Clients;
using WeatherForecastExample.ApplicationCore.Models;

namespace WeatherForecastExample.ApplicationCore.Services;

public interface IWeatherForecastService
{
    Task<OneOf<WeatherForecastResult, WeatherForecastError>> GetForecasts(string locationName,
        CancellationToken cancellationToken);
}

public class WeatherForecastService : IWeatherForecastService
{
    private readonly IGeoCodingClient _geoCodingClient;
    private readonly IWeatherClient _weatherClient;
    private readonly IOpenMeteoMappingService _mappingService;

    public WeatherForecastService(
        IGeoCodingClient geoCodingClient,
        IWeatherClient weatherClient,
        IOpenMeteoMappingService mappingService)
    {
        _geoCodingClient = geoCodingClient;
        _weatherClient = weatherClient;
        _mappingService = mappingService;
    }

    public async Task<OneOf<WeatherForecastResult, WeatherForecastError>> GetForecasts(string locationName,
        CancellationToken cancellationToken)
    {
        try
        {
            var locationResult = await _geoCodingClient.GetLocation(locationName, cancellationToken);

            var location = locationResult.Results!.First();

            var weatherResponse = await _weatherClient.GetWeatherForecasts(
                latitude: location.Latitude,
                longitude: location.Longitude,
                cancellationToken: cancellationToken
            );

            return _mappingService.Map(weatherResponse, location);
        }
        catch (ClientException exception)
        {
            // We know this message is user safe
            return new WeatherForecastError(exception.Message);
        }
    }
}