using Northwind.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.WebHost.UseUrls("https://localhost:5003");

builder.Services.AddCors();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// pozwól na obs³ugê wy³¹cznie klienta MVC i tylko na ¿¹dania GET
app.UseCors(configurePolicy: options =>
{
	options.WithMethods("GET");
	options.WithOrigins("https://localhost:5001");
});

app.MapGet("/api/weather", () =>
{
	return Enumerable.Range(1, 5).Select(index =>
	  new WeatherForecast
	  {
		  Date = DateTime.Now.AddDays(index),
		  TemperatureC = Random.Shared.Next(-20, 55),
		  Summary = WeatherForecast.Summaries[
		  Random.Shared.Next(WeatherForecast.Summaries.Length)]
	  })
	  .ToArray();
});

app.Run();
