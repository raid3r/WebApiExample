using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApiExample.Models;

namespace WebApiExample.Controllers;

[ApiController]
[Route("api/v1/weather-forecast")]
public class WeatherForecastController(
    ILogger<WeatherForecastController> logger
    ) : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };


    [HttpGet(Name = "GetAllWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Id = index,
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    /// <summary>
    /// Get one weather forecast by id
    /// </summary>
    /// <param name="id">Id of forecast</param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "GetOneWeatherForecastById")]
    public WeatherForecast One(int id)
    {
        
        logger.LogInformation("Get forecast" + id.ToString());

        return new WeatherForecast
        {
            Id = id,
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(id)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        };
    }

    [HttpPut]
    public AddEntityResult Create(WeatherForecast weatherForecast)
    {
        throw new BadHttpRequestException("Invalid data");

        weatherForecast.Id = 111111;
        // save changes
        return new AddEntityResult { Ok = true, Id = weatherForecast.Id };
    }
}


