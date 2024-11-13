using CommonLibrary; // extension method AddNorthwindContextSqlite()

namespace Northwind.Web;

public class Startup
{
	// Obie metody zostaną automatycznie wywołane w trakcie pracy aplikacji.

	// Dodaje serwisy zależne do kontenera wstrzykiwania zależności, używanego na przykład przez:
	// Razor Pages, funkcje CORS (ang. Cross-Origin Resource Sharing) albo kontekst bazy danych umożliwiający dostęp do bazy
	public void ConfigureServices(IServiceCollection services)
	{
		// instrukcja włączająca mechanizmy stron Razor Pages oraz powiązane z nimi serwisy,
		// takie jak wiązanie modeli, autoryzacja, zabezpieczenia, widoki oraz pomocnicze znaczniki
		services.AddRazorPages();

		// instrukcja rejestrująca klasę kontekstu bazy danych Northwind
		services.AddNorthwindContextSqlite();
	}

	// Definiuje potok obsługi żądań HTTP, ustalając przepływ żądań i odpowiedzi.
	// Można tu wywoływać różne metody Use pochodzące z parametru app,
	// aby zbudować potok realizujący wybrane funkcje w odpowiedniej kolejności
	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (!env.IsDevelopment())
		{
			app.UseHsts();
		}

		// Uruchamia routing dla punktu końcowego
		// oznacza pozycję w potoku, na której podejmowane są decyzje dotyczące routowania
		app.UseRouting();

		app.Use(async (HttpContext context, Func<Task> next) =>
		{
			RouteEndpoint? rep = context.GetEndpoint() as RouteEndpoint;
			if (rep is not null)
			{
				Console.WriteLine($"Nazwa punktu końcowego: {rep.DisplayName}");
				Console.WriteLine($"Wzorzec scieżki punktu końcowego: {rep.RoutePattern.RawText}");
			}

			if (context.Request.Path == "/bonjour")
			{
				// W przypadku pasującej ścieżki URL, ta metoda zakończy potok i w związku z tym nie będzie wywoływać następnego delegata
				await context.Response.WriteAsync("Bonjour Monde!");
				return;
			}

			// Możemy tu zmienić żądanie przed przekazaniem go do następnego delegata
			await next();
			// Możemy tu zmienić odpowiedź przed przekazaniem jej do następnego delegata
		});

		// Przekierowywanie z http na zabezpieczone połączenie https
		app.UseHttpsRedirection();

		// Obsługa plików domyślnych
		// Wywołanie metody UseDefaultFiles musi się znaleźć przed wywołaniem metody UseStaticFiles. W przeciwnym wypadku nie będzie ona działać.
		app.UseDefaultFiles(); // index.html, default.html itd.

		// Włączenie obsługi plików statycznych
		app.UseStaticFiles();

		// Definiuje trasę oczekującą żądań HTTP GET dla katalogu głównego (/),
		// dla których tworzona jest standardowa odpowiedź składająca się z prostego tekstu „Witaj, świecie!”.
		// oznacza pozycję w potoku, na której wybierany jest punkt końcowy
		app.UseEndpoints(endpoints => 
		{
			// instrukcja włączająca mechanizmy stron Razor Pages
			endpoints.MapRazorPages();

			//endpoints.MapGet("/", () => "Witaj, świecie!");
			endpoints.MapGet("/witaj", () => "Witaj, świecie!"); 
		});
	}
}