using System.Text.Json.Serialization;

namespace WeatherForecastExample.ApplicationCore.Clients.Contracts;

/// <summary>
/// Database entity
/// </summary>
public class OpenMeteoWeatherResponse
{
    [JsonPropertyName("generationtime_ms")]
    public decimal? GenerationTimeMs { get; init; }

    [JsonPropertyName("utc_offset_seconds")]
    public int? UtcOffsetSeconds { get; init; }

    public string? TimeZone { get; init; }

    [JsonPropertyName("timezone_abbreviation")]
    public string? TimeZoneAbbreviation { get; init; }

    public float? Elevation { get; init; }

    [JsonPropertyName("hourly_units")]
    public OpenMeteoHourlyUnits? HourlyUnits { get; init; }

    public OpenMeteoHourly? Hourly { get; init; }
}

public class OpenMeteoHourlyUnits
{
    public string? Time { get; init; }

    [JsonPropertyName("temperature_2m")]
    public string? Temperature2M { get; init; }
}

public class OpenMeteoHourly
{
    [JsonPropertyName("Time")]
    public IEnumerable<DateTime>? UtcTime { get; init; }

    [JsonPropertyName("temperature_2m")]
    public IEnumerable<decimal>? Temperature2M { get; init; }
}