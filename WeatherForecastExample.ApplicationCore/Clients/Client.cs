using System.Text.Json;

namespace WeatherForecastExample.ApplicationCore.Clients;

public abstract class Client
{
    protected static async Task<T?> DeserializeResult<T>(HttpResponseMessage responseMessage,
        JsonSerializerOptions jsonSerializerOptions, CancellationToken cancellationToken)
    {
        await using var responseStream = await responseMessage.Content.ReadAsStreamAsync(cancellationToken);

        return await JsonSerializer.DeserializeAsync<T>(responseStream, jsonSerializerOptions,
            cancellationToken);
    }
}