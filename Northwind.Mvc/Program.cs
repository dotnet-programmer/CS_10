using Microsoft.AspNetCore.Identity; // IdentityUser
using Microsoft.EntityFrameworkCore; // UseSqlServer, UseSqlite
using Northwind.Mvc.Data; // ApplicationDbContext
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

var app = builder.Build();

// Configure the HTTP request pipeline. (Konfigurowanie potoku obs³ugi ¿¹dañ HTTP)
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

app.Run();