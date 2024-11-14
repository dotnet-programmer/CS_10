using Microsoft.AspNetCore.Identity; // IdentityUser
using Microsoft.EntityFrameworkCore; // UseSqlServer, UseSqlite
using Northwind.Mvc.Data; // ApplicationDbContext
using CommonLibrary;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configuration � zawiera warto�ci zebrane z wszystkich miejsc, w kt�rych mo�na zapisywa� konfiguracj� witryny: pliku appsettings.json, zmiennych �rodowiskowych, parametr�w wiersza polece� itd.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
// Services � jest kolekcj� zarejestrowanych serwis�w zale�nych.
//Wywo�anie metody AddDbContext jest przyk�adem rejestrowania zale�no�ci od�serwisu. 
//ASP.NET Core implementuje wzorzec projektowy wstrzykiwania zale�no�ci (ang. dependency injection � DI), dzi�ki czemu kontrolery mog� w�konstruktorach ��da� potrzebnych im serwis�w. 
//Programi�ci musz� jedynie zarejestrowa� te serwisy w tej cz�ci pliku Program.cs. (A je�eli u�ywasz klasy Startup, to w metodzie ConfigureServices).
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
	.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddRoles<IdentityRole>() // w��cza zarz�dzanie rolami
	.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
// dodawanie w�asnych filtr�w globalnie:
//builder.Services.AddControllersWithViews(opcje => { opcje.Filters.Add(typeof(MojWlasnyFiltr)); });

// Je�eli u�ywasz bazy SQLite, domy�lna �cie�ka to ..\Northwind.db
//builder.Services.AddNorthwindContextSqlite();
string? sqlServerConnection = builder.Configuration.GetConnectionString("NorthwindConnection");
builder.Services.AddNorthwindContextSqlServer(sqlServerConnection);

var app = builder.Build();

// Configure the HTTP request pipeline. (Konfigurowanie potoku obs�ugi ��da� HTTP)
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