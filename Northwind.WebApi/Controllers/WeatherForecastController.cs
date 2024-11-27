using Microsoft.AspNetCore.Mvc;

namespace Northwind.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
	private static readonly string[] Summaries = new[]
	{
		"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
	};

	private readonly ILogger<WeatherForecastController> _logger;

	public WeatherForecastController(ILogger<WeatherForecastController> logger)
	{
		_logger = logger;
	}

	// Atrybut [HttpGet] rejestruje w kontrolerze metodê Get, która bêdzie reagowa³a na ¿¹dania HTTP GET
	//[HttpGet(Name = "GetWeatherForecast")]
	//public IEnumerable<WeatherForecast> Get()
	//{
	//	return Enumerable.Range(1, 5).Select(index => new WeatherForecast
	//	{
	//		Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
	//		TemperatureC = Random.Shared.Next(-20, 55),
	//		Summary = Summaries[Random.Shared.Next(Summaries.Length)]
	//	})
	//	.ToArray();
	//}

	// GET /weatherforecast
	[HttpGet]
	public IEnumerable<WeatherForecast> Get()  // pierwotna metoda
	{
		return Get(5);  // prognoza dla piêciu dni
	}

	// GET /weatherforecast/7
	[HttpGet("{days:int}")]
	public IEnumerable<WeatherForecast> Get(int days)  // nowa metoda
	{
		return Enumerable.Range(1, days).Select(index => new WeatherForecast
		  {
			  Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
			  TemperatureC = Random.Shared.Next(-20, 55),
			  Summary = Summaries[Random.Shared.Next(Summaries.Length)]
		  })
		 .ToArray();
	}
}
