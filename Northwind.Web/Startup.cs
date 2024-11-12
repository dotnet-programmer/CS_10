namespace Northwind.Web;

public class Startup
{
	// Obie metody zostaną automatycznie wywołane w trakcie pracy aplikacji.

	// Dodaje serwisy zależne do kontenera wstrzykiwania zależności, używanego na przykład przez:
	// Razor Pages, funkcje CORS (ang. Cross-Origin Resource Sharing) albo kontekst bazy danych umożliwiający dostęp do bazy
	public void ConfigureServices(IServiceCollection services)
	{
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
		app.UseRouting();

		// Przekierowywanie z http na zabezpieczone połączenie https
		app.UseHttpsRedirection();

		// Obsługa plików domyślnych
		// Wywołanie metody UseDefaultFiles musi się znaleźć przed wywołaniem metody UseStaticFiles. W przeciwnym wypadku nie będzie ona działać.
		app.UseDefaultFiles(); // index.html, default.html itd.

		// Włączenie obsługi plików statycznych
		app.UseStaticFiles();

		// Definiuje trasę oczekującą żądań HTTP GET dla katalogu głównego (/),
		// dla których tworzona jest standardowa odpowiedź składająca się z prostego tekstu „Witaj, świecie!”.
		//app.UseEndpoints(endpoints => endpoints.MapGet("/", () => "Witaj, świecie!"));
		
		app.UseEndpoints(endpoints => endpoints.MapGet("/witaj", () => "Witaj, świecie!"));
	}
}