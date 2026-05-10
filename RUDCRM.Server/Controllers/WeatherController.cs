using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RUDCRM.Server.Services;
using RUDCRM.Server.DTOs;

namespace RUDCRM.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class WeatherController : ControllerBase
{
    private readonly WeatherService _weatherService;

    public WeatherController(WeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet("{city}")]
    public async Task<ActionResult<WeatherDto>> GetWeather(string city)
    {
        var weather = await _weatherService.GetWeatherAsync(city);
        if (weather == null)
            return NotFound(new { message = "Could not fetch weather data. Check API key or city name." });

        return Ok(weather);
    }
}
