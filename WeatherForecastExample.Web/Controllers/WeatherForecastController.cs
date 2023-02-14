using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using WeatherForecastExample.ApplicationCore.Models;
using WeatherForecastExample.ApplicationCore.Services;

namespace WeatherForecastExample.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherForecastService _weatherForecastService;

    public WeatherForecastController(IWeatherForecastService weatherForecastService)
    {
        _weatherForecastService = weatherForecastService;
    }

    [HttpGet]
    public async Task<ActionResult<WeatherForecastResult>> Get([FromQuery] [Required] string search, CancellationToken cancellationToken)
    {
        var result = await _weatherForecastService.GetForecasts(search, cancellationToken);
        return Ok(result);
    }
}