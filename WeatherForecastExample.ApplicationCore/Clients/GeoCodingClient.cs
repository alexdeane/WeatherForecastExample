using System.Net.Http.Json;
using System.Text.Json;
using WeatherForecastExample.ApplicationCore.Clients.Contracts;

namespace WeatherForecastExample.ApplicationCore.Clients;

public interface IGeoCodingClient
{
    Task<OpenMeteoLocationResponse> GetLocation(string name, CancellationToken cancellationToken);
}

// This and the weather client do almost identical operations and one may be tempted
// to consolidate them into the same class - but I've found that keeping clients separate
// is usually easier to maintain, especially in larger applications
public class GeoCodingClient : IGeoCodingClient
{
    private readonly IHttpClientFactory _factory;
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);

    public GeoCodingClient(IHttpClientFactory factory)
    {
        _factory = factory;
    }

    public async Task<OpenMeteoLocationResponse> GetLocation(string name, CancellationToken cancellationToken)
    {
        var uri = BuildUri(name);

        var client = _factory.CreateClient(nameof(GeoCodingClient));

        try
        {
            var result = await client.GetFromJsonAsync<OpenMeteoLocationResponse>(
                requestUri: uri, 
                options: JsonSerializerOptions,
                cancellationToken: cancellationToken
            );

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

    private static Uri BuildUri(string search) => new($"v1/search?name={search}");
}