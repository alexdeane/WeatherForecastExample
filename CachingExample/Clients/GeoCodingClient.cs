using System.Text.Json;
using CachingExample.Context.Entities;

namespace CachingExample.Clients;

public class GeoCodingClient : Client
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);

    public async Task<OpenMeteoLocation?> GetLocation(string name)
    {
        var uri = BuildUri(name);

        using var client = new HttpClient
        {
            BaseAddress = uri
        };

        using var response = await client.GetAsync(uri);
        return await DeserializeResult<OpenMeteoLocation>(response, JsonSerializerOptions);
    }

    private static Uri BuildUri(string search) => new($"https://geocoding-api.open-meteo.com/v1/search?name={search}");
}