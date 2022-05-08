using Microsoft.AspNetCore.Mvc;
using CorePGIdentityTest.Entities;
using CorePGIdentityTest.Data;
using Microsoft.AspNetCore.Authorization;

namespace CorePGIdentityTest.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ApiDbContext _apiDbContext;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, ApiDbContext apiDbContext)
    {
        _logger = logger;
        _apiDbContext = apiDbContext;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_apiDbContext.WeatherForecasts);
    }

    [HttpPost]
    public async Task<IActionResult> Post(int temp, string summary)
    {
        WeatherForecast weatherForecast = new WeatherForecast()
        {
            Date = DateTime.Now.ToUniversalTime(),
            TemperatureC = temp,
            Summary = summary
        };

        await _apiDbContext.WeatherForecasts.AddAsync(weatherForecast);
        await _apiDbContext.SaveChangesAsync();
        return Ok(weatherForecast);
    }
}
