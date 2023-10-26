using WeatherForecastExample.ApplicationCore.Clients;
using WeatherForecastExample.ApplicationCore.Services;

namespace WeatherForecastExample.Web;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddScoped<IGeoCodingClient, GeoCodingClient>();
        services.AddScoped<IWeatherClient, WeatherClient>();
        services.AddScoped<IOpenMeteoMappingService, OpenMeteoMappingService>();
        services.AddScoped<IWeatherForecastService, WeatherForecastService>();

        services.AddHttpClient<WeatherClient>(client =>
        {
            // The URL is hardcoded but obviously in a real scenario you would make this configurable
            client.BaseAddress = new Uri("https://api.open-meteo.com/");
        });

        services.AddHttpClient<GeoCodingClient>(client =>
        {
            client.BaseAddress = new Uri("https://geocoding-api.open-meteo.com/");
        });
    }
}