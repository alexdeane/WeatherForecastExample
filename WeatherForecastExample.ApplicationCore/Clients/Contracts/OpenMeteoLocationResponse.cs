// ReSharper disable ClassNeverInstantiated.Global

namespace WeatherForecastExample.ApplicationCore.Clients.Contracts;

/// <summary>
/// POCO for the response from the GeoCoding API
/// </summary>
public class OpenMeteoLocationResponse
{
    public IEnumerable<OpenMeteoLocationResult>? Results { get; init; }
}

public class OpenMeteoLocationResult
{
    public long Id { get; init; }
    public string? Name { get; init; }
    public string? Country { get; init; }
    public string? TimeZone { get; init; }
    public decimal Latitude { get; init; }
    public decimal Longitude { get; init; }
}