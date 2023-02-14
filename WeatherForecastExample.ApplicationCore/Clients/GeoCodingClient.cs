using System.Text.Json;
using WeatherForecastExample.ApplicationCore.Clients.Contracts;

namespace WeatherForecastExample.ApplicationCore.Clients;

public interface IGeoCodingClient
{
    Task<OpenMeteoLocationResponse> GetLocation(string name, CancellationToken cancellationToken);
}

public class GeoCodingClient : Client, IGeoCodingClient
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);

    public async Task<OpenMeteoLocationResponse> GetLocation(string name, CancellationToken cancellationToken)
    {
        var uri = BuildUri(name);

        using var client = new HttpClient
        {
            BaseAddress = uri
        };

        try
        {
            using var response = await client.GetAsync(uri, cancellationToken);
            
            var result = await DeserializeResult<OpenMeteoLocationResponse>(response, JsonSerializerOptions,
                cancellationToken);

            if (result?.Results is null || !result.Results.Any())
                throw new ClientException("Location not found");

            return result;
        }
        catch (Exception exception) when (exception is not ClientException)
        {
            // Normally you would want to log the original exception
            throw new ClientException("Error occurred finding location", exception);
        }
    }

    private static Uri BuildUri(string search) => new($"https://geocoding-api.open-meteo.com/v1/search?name={search}");
}