using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Northwind.BlazorServer.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// konfiguruje aplikacjê ASP.NET Core tak, ¿eby przyjmowa³a po³¹czenia SignalR z komponentów Blazora
// i definiuje awaryjn¹ stronê Razora o nazwie _Host.cshtml
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
