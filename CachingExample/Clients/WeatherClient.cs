using System.Text.Json;
using CachingExample.Context.Entities;

namespace CachingExample.Clients;

/// <summary>
/// Client for getting data from weather thing
/// </summary>
public class WeatherClient : Client
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);

    /// <summary>
    /// Gets weather forecasts from
    /// a free API
    /// </summary>
    private async Task<IEnumerable<WeatherForecast>?> GetWeatherForecasts(decimal latitude, decimal longitude)
    {
        var uri = BuildUri(latitude, longitude);

        using var client = new HttpClient
        {
            BaseAddress = uri
        };

        using var response = await client.GetAsync(uri);
        return await DeserializeResult<IEnumerable<WeatherForecast>>(response, JsonSerializerOptions);
    }
    private static Uri BuildUri(decimal latitude, decimal longitude) =>
        new($"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&hourly=temperature_2m&windspeed_unit=mph&precipitation_unit=inch");
}