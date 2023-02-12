using CachingExample.Context;
using CachingExample.Context.Entities;
using Microsoft.EntityFrameworkCore;

namespace CachingExample.Repositories;

public interface IWeatherForecastRepository
{
    Task<IEnumerable<WeatherForecast>> GetWeatherForecasts();
}

public class WeatherForecastRepository : IWeatherForecastRepository
{
    private readonly WeatherForecastContext _context;

    public WeatherForecastRepository(WeatherForecastContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WeatherForecast>> GetWeatherForecasts()
    {
        return await _context.Set<WeatherForecast>()
            .ToArrayAsync();
    }
}