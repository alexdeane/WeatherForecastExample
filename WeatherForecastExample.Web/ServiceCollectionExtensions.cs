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
    }
}