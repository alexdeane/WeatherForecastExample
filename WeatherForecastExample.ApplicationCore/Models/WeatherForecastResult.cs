namespace WeatherForecastExample.ApplicationCore.Models;

/// <summary>
/// Our DTO for the frontend 
/// </summary>
public class WeatherForecastResult
{
    public string? Name { get; init; }
    public string? Country { get; init; }
    public string? TimeZone { get; init; }
    public IEnumerable<WeatherForecastData>? Data { get; init; }
    public string? TimeUnit { get; init; }
    public string? TemperatureUnit { get; init; }
}

public class WeatherForecastData
{
    public DateTime? Date { get; init; }
    public decimal[]? Temperatures { get; init; }
}