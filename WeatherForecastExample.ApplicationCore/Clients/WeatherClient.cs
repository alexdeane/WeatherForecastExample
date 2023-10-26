using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Json;
using WeatherForecastExample.ApplicationCore.Clients.Contracts;

namespace WeatherForecastExample.ApplicationCore.Clients;

public interface IWeatherClient
{
    Task<OpenMeteoWeatherResponse> GetWeatherForecasts(decimal latitude, decimal longitude,
        CancellationToken cancellationToken);
}

/// <summary>
/// Client for getting data from weather thing
/// </summary>
public class WeatherClient : IWeatherClient
{
    private readonly IHttpClientFactory _factory;
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);

    public WeatherClient(IHttpClientFactory factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Gets weather forecast data from a free API
    /// </summary>
    public async Task<OpenMeteoWeatherResponse> GetWeatherForecasts(decimal latitude, decimal longitude,
        CancellationToken cancellationToken)
    {
        // 1. Don't dispose a client created from the IHttpClientFactory - allow the factory to keep and reuse it
        // 2. Instantiate the client in your method, not your constructor. Doing this prevents the client from
        //    being instantiated at all if for whatever reason the request does not make it to this point.
        var client = _factory.CreateClient(nameof(WeatherClient));
        
        var uri = BuildUri(latitude, longitude);

        try
        {
            var result = await client.GetFromJsonAsync<OpenMeteoWeatherResponse>(
                requestUri: uri,
                options: JsonSerializerOptions,
                cancellationToken: cancellationToken
            );

            if (result is null)
                throw new ClientException("Failed to get weather data.");

            return result;
        }
        catch (Exception exception) when (exception is not ClientException)
        {
            throw new ClientException("Failed to get weather data", exception);
        }
    }

    private static string BuildUri(decimal latitude, decimal longitude) =>
            $"v1/forecast?latitude={latitude}&longitude={longitude}&hourly=temperature_2m&windspeed_unit=mph&precipitation_unit=inch&temperature_unit=fahrenheit";
}