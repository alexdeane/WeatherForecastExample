using System.Text.Json;
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
public class WeatherClient : Client, IWeatherClient
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);

    /// <summary>
    /// Gets weather forecasts from
    /// a free API
    /// </summary>
    public async Task<OpenMeteoWeatherResponse> GetWeatherForecasts(decimal latitude, decimal longitude,
        CancellationToken cancellationToken)
    {
        var uri = BuildUri(latitude, longitude);

        using var client = new HttpClient
        {
            BaseAddress = uri
        };

        try
        {
            using var response = await client.GetAsync(uri, cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new ClientException("Failed to get weather data.");

            var result =
                await DeserializeResult<OpenMeteoWeatherResponse>(response, JsonSerializerOptions, cancellationToken);

            if (result is null)
                throw new ClientException("Failed to get weather data.");

            return result;
        }
        catch (Exception exception) when (exception is not ClientException)
        {
            throw new ClientException("Failed to get weather data", exception);
        }
    }

    private static Uri BuildUri(decimal latitude, decimal longitude) =>
        new(
            $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&hourly=temperature_2m&windspeed_unit=mph&precipitation_unit=inch&temperature_unit=fahrenheit");
}