using WeatherForecastExample.ApplicationCore.Abstraction;
using WeatherForecastExample.ApplicationCore.Clients;
using WeatherForecastExample.ApplicationCore.Clients.Contracts;
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

    public WeatherForecastService(
        IGeoCodingClient geoCodingClient,
        IWeatherClient weatherClient,
        IOpenMeteoMappingService mappingService)
    {
        _geoCodingClient = geoCodingClient;
        _weatherClient = weatherClient;
        _mappingService = mappingService;
    }

    public async Task<ServiceResult<WeatherForecastResult>> GetForecasts(string locationName,
        CancellationToken cancellationToken)
    {
        OpenMeteoLocationResponse locationResult;

        try
        {
            locationResult = await _geoCodingClient.GetLocation(locationName, cancellationToken);
        }
        catch (ClientException exception)
        {
            // We know this message is user safe
            return ErrorResult(exception.Message);
        }

        var location = locationResult.Results!.First();

        OpenMeteoWeatherResponse weatherResponse;

        try
        {
            weatherResponse = await _weatherClient.GetWeatherForecasts(
                latitude: location.Latitude,
                longitude: location.Longitude,
                cancellationToken: cancellationToken
            );
        }
        catch (ClientException exception)
        {
            // We know this message is user safe
            return ErrorResult(exception.Message);
        }

        var result = _mappingService.Map(weatherResponse, location);

        return new ServiceResult<WeatherForecastResult>(result);
    }

    private static ServiceResult<WeatherForecastResult> ErrorResult(string errorMessage)
        => new()
        {
            UserSafeError = errorMessage
        };
}