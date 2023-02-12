namespace CachingExample.Context.Entities;

/// <summary>
/// client result from geolocation
/// </summary>
public class OpenMeteoLocation
{
    public IEnumerable<OpenMeteoLocationResult> Results { get; init; }
}

public class OpenMeteoLocationResult
{
    public long Id { get; init; }
    public string Name { get; init; }
    public decimal Latitude { get; init; }
    public decimal Longitude { get; init; }
}