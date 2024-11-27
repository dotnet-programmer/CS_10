using Microsoft.AspNetCore.Mvc.Formatters;
using CommonLibrary;
using Northwind.WebApi.Reposiories;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.AspNetCore.HttpLogging; // HttpLoggingFields
using Northwind.WebApi; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.WebHost.UseUrls("https://localhost:5002/");

builder.Services.AddNorthwindContextSqlServer();

builder.Services.AddControllers(options =>
	{
		Console.WriteLine("Domyœlne formatery wyjœcia:");
		foreach (IOutputFormatter formatter in options.OutputFormatters)
		{
			OutputFormatter? mediaFormatter = formatter as OutputFormatter;
			if (mediaFormatter == null)
			{
				Console.WriteLine($" {formatter.GetType().Name}");
			}
			else // klasa OutputFormatter ma w³aœciwoœæ SupportedMediaTypes
			{
				Console.WriteLine(" {0}, typy mediów: {1}",
				  arg0: mediaFormatter.GetType().Name,
				  arg1: string.Join(", ", mediaFormatter.SupportedMediaTypes));
			}
		}
	})
	.AddXmlDataContractSerializerFormatters()
	.AddXmlSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new() { Title = "API serwisu Northwind", Version = "v1" });
});

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddHttpLogging(options =>
{
	options.LoggingFields = HttpLoggingFields.All;
	options.RequestBodyLogLimit = 4096; // domyœlnie 32k
	options.ResponseBodyLogLimit = 4096; // domyœlnie 32k
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpLogging();

// Dodawanie zabezpieczaj¹cych nag³ówków HTTP
app.UseMiddleware<SecurityHeaders>();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "API serwisu Northwind, wersja 1");
		c.SupportedSubmitMethods(new[] { SubmitMethod.Get, SubmitMethod.Post, SubmitMethod.Put, SubmitMethod.Delete });
	});
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
