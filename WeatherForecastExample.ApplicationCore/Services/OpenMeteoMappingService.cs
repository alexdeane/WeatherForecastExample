using WeatherForecastExample.ApplicationCore.Clients.Contracts;
using WeatherForecastExample.ApplicationCore.Models;

namespace WeatherForecastExample.ApplicationCore.Services;

/// <summary>
/// This service exists to map the OpenMeteo Weather API
/// response into a more usable form for the frontend.
/// </summary>
public interface IOpenMeteoMappingService
{
    public WeatherForecastResult Map(OpenMeteoWeatherResponse openMeteoWeatherResponse,
        OpenMeteoLocationResult location);
}

/// <summary>
/// This logic is nice to have in its own service simply
/// because it makes unit testing easier.
/// </summary>
public class OpenMeteoMappingService : IOpenMeteoMappingService
{
    public WeatherForecastResult Map(
        OpenMeteoWeatherResponse openMeteoWeatherResponse,
        OpenMeteoLocationResult location)
    {
        var temperatures = openMeteoWeatherResponse.Hourly?.Temperature2M;
        var dates = openMeteoWeatherResponse.Hourly?.UtcTime;

        return new WeatherForecastResult
        {
            Name = location.Name,
            Country = location.Country,
            TimeZone = location.TimeZone,
            TimeUnit = openMeteoWeatherResponse.HourlyUnits?.Time,
            TemperatureUnit = openMeteoWeatherResponse.HourlyUnits?.Temperature2M,
            Data = MapForecastData(dates, temperatures)
        };
    }

    // This just reworks the API response such that it's a list
    // of objects, each with a Date and an array of 24 temperatures.
    // The index of the array indicates the hour of the day 
    private static IEnumerable<WeatherForecastData> MapForecastData(
        IEnumerable<DateTime> dates,
        IEnumerable<decimal> temperatures)
        => temperatures
            .Zip(dates, (temp, t) =>
                new
                {
                    Temperature = temp,
                    Date = t.Date,
                    Time = t.Date.TimeOfDay
                }
            )
            .GroupBy(x => x.Date)
            .ToDictionary(x => x.Key, x => x.OrderBy(y => y.Time).ToArray())
            .Select(x => new WeatherForecastData
            {
                Temperatures = x.Value.Select(y => y.Temperature).ToArray(),
                Date = x.Key
            });
}