using System.Text.Json;
namespace CachingExample.Clients;

public abstract class Client
{
    protected static async Task<T?> DeserializeResult<T>(HttpResponseMessage responseMessage,
        JsonSerializerOptions jsonSerializerOptions)
    {
        await using var responseStream = await responseMessage.Content.ReadAsStreamAsync();

        return await JsonSerializer.DeserializeAsync<T>(responseStream, jsonSerializerOptions,
            CancellationToken.None);
    }
}