using Microsoft.AspNetCore.Identity; // IdentityUser
using Microsoft.EntityFrameworkCore; // UseSqlServer, UseSqlite
using Northwind.Mvc.Data; // ApplicationDbContext
using System.Net.Http.Headers; // MediaTypeWithQualityHeaderValue
using CommonLibrary;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configuration — zawiera wartoœci zebrane z wszystkich miejsc, w których mo¿na zapisywaæ konfiguracjê witryny: pliku appsettings.json, zmiennych œrodowiskowych, parametrów wiersza poleceñ itd.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
// Services — jest kolekcj¹ zarejestrowanych serwisów zale¿nych.
//Wywo³anie metody AddDbContext jest przyk³adem rejestrowania zale¿noœci od serwisu. 
//ASP.NET Core implementuje wzorzec projektowy wstrzykiwania zale¿noœci (ang. dependency injection — DI), dziêki czemu kontrolery mog¹ w konstruktorach ¿¹daæ potrzebnych im serwisów. 
//Programiœci musz¹ jedynie zarejestrowaæ te serwisy w tej czêœci pliku Program.cs. (A je¿eli u¿ywasz klasy Startup, to w metodzie ConfigureServices).
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
	.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddRoles<IdentityRole>() // w³¹cza zarz¹dzanie rolami
	.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
// dodawanie w³asnych filtrów globalnie:
//builder.Services.AddControllersWithViews(opcje => { opcje.Filters.Add(typeof(MojWlasnyFiltr)); });

// Je¿eli u¿ywasz bazy SQLite, domyœlna œcie¿ka to ..\Northwind.db
//builder.Services.AddNorthwindContextSqlite();
string? sqlServerConnection = builder.Configuration.GetConnectionString("NorthwindConnection");
builder.Services.AddNorthwindContextSqlServer(sqlServerConnection);

// instrukcja u¿ywaj¹ca klasê HttpClientFactory w po³¹czeniu z nazw¹ klienta,
// za pomoc¹ której bêd¹ wykonywane wywo³ania serwisu Northwind Web API poprzez protokó³ HTTPS na porcie 5002.
// Wybrano te¿ format JSON jako domyœlny format odpowiedzi
builder.Services.AddHttpClient(name: "Northwind.WebApi", configureClient: options =>
  {
	  options.BaseAddress = new Uri("https://localhost:5002/");
	  options.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json", 1.0));
  });

// instrukcja konfiguruj¹ca klienta HTTP wywo³uj¹cego serwis minimalny na porcie 5003
builder.Services.AddHttpClient(name: "Minimal.WebApi", configureClient: options =>
  {
	  options.BaseAddress = new Uri("https://localhost:5003/");
	  options.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json", 1.0));
  });

builder.Services.AddCors();

// instrukcja dodaj¹ca do projektu kontrole stanu systemu, w tym kontrole kontekstu bazy danych Northwind
builder.Services.AddHealthChecks().AddDbContextCheck<NorthwindContext>();

var app = builder.Build();

// Configure the HTTP request pipeline. (Konfigurowanie potoku obs³ugi ¿¹dañ HTTP)

app.UseCors(configurePolicy: options =>
{
	options.WithMethods("GET", "POST", "PUT", "DELETE");
	options.WithOrigins("https://localhost:5001"); // umo¿liwia obs³ugê ¿¹dañ od klienta MVC
});

if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.UseHealthChecks(path: "/howdoyoufeel");

app.Run();