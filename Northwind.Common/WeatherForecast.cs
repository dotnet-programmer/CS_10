namespace Northwind.Common;

public class WeatherForecast
{
	public static readonly string[] Summaries = new[]
	{
	  "Mroźno", "Zimno", "Chłodno", "Chłodnawo", "Przyjemnie", "Ciepławo", "Ciepło", "Gorąco", "Upalnie", "Żar z nieba"
	};

	public DateTime Date { get; set; }

	public int TemperatureC { get; set; }

	public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

	public string? Summary { get; set; }
}