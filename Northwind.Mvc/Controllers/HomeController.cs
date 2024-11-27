using System.Diagnostics; // Activity
using Microsoft.AspNetCore.Mvc; // Controller, IActionResult
using Northwind.Mvc.Models; // ErrorViewModel
using Microsoft.AspNetCore.Authorization;
using CommonLibrary;
using Microsoft.EntityFrameworkCore;
using Northwind.Common;

namespace Northwind.Mvc.Controllers;
public class HomeController : Controller
{
	// Deklarowane jest prywatne pole przechowuj¹ce referencjê obiektu protoko³uj¹cego, który jest przekazywany kontrolerowi HomeController w konstruktorze.
	private readonly ILogger<HomeController> _logger;

	private readonly NorthwindContext _northwindContext;
	private readonly IHttpClientFactory _httpClientFactory;

	// Okreœlenie w konstruktorze klasy serwisów wymaganych przez kontroler w celu osi¹gniêcia poprawnego stanu, który umo¿liwia mu normaln¹ pracê.
	public HomeController(ILogger<HomeController> logger, NorthwindContext context, IHttpClientFactory clientFactory)
	{
		_logger = logger;
		_northwindContext = context;
		_httpClientFactory = clientFactory;
	}

	// Wszystkie trzy metody akcji wywo³uj¹ metodê View() i zwracaj¹ otrzymany od niej wynik typu IActionResult jako odpowiedŸ dla klienta.
	public async Task<IActionResult> Index()
	{
		_logger.LogError("Naprawdê powa¿ny b³¹d (tylko ¿artujê!)");
		_logger.LogWarning("To jest pierwsze ostrze¿enie!");
		_logger.LogWarning("Drugie ostrze¿enie!");
		_logger.LogInformation("Jestem metod¹ Index kontrolera HomeController.");

		try
		{
			HttpClient client = _httpClientFactory.CreateClient(name: "Minimal.WebApi");
			HttpRequestMessage request = new(method: HttpMethod.Get, requestUri: "api/weather");
			HttpResponseMessage response = await client.SendAsync(request);
			ViewData["weather"] = await response.Content.ReadFromJsonAsync<WeatherForecast[]>();
		}
		catch (Exception ex)
		{
			_logger.LogWarning($"The Minimal.WebApi service is not responding. Exception: {ex.Message}");
			ViewData["weather"] = Enumerable.Empty<WeatherForecast>().ToArray();
		}

		HomeIndexViewModel model = new()
		{
			NumberOfVisits = (new Random()).Next(1, 1001),
			Categories = await _northwindContext.Categories.ToListAsync(),
			Products = await _northwindContext.Products.ToListAsync()
		};

		// przekazanie modelu do widoku
		return View(model);
	}

	// Mo¿emy te¿ chcieæ zapisywaæ w pamiêci podrêcznej odpowiedzi HTTP generowane przez akcjê modelu. W tym celu nale¿y dopisaæ do metody atrybut [ResponseCache].
	// Mo¿emy zdecydowaæ, gdzie i jak d³ugo odpowiedŸ ma byæ przechowywana w pamiêci podrêcznej, stosuj¹c do tego parametry podane na poni¿szej liœcie:
	// Duration — wartoœæ podawana w sekundach.Ustala wartoœæ nag³ówka HTTP o nazwie max-age.
	// Location — jedna z wartoœci typu ResponseCacheLocation: Any, Client lub None.Ustala wartoœæ nag³ówka HTTP o nazwie cache-control.
	// NoStore — je¿eli ma wartoœæ true, to parametry Duration i Location s¹ ignorowane, a nag³ówkowi HTTP o nazwie cache-control przypisywana jest wartoœæ no-store.
	// Oznacza to, ¿e przez podany czas przegl¹darka przy ka¿dym wejœciu bêdzie u¿ywa³a zapamiêtanej strony zamis ³adowaæ j¹ od nowa
	[ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
	public IActionResult IndexWithCache()
	{
		return View("Index");
	}

	// Metody Index i Privacy maj¹ jednakow¹ implementacjê, a mimo to zwracaj¹ odmienne strony WWW. Wynika to z zastosowania konwencji.
	// W obu przypadkach metoda View poszukuje plików Razor u¿ywanych do generowania strony WWW, ale szuka ich w innych œcie¿kach:
	// Konkretny widok strony Razor: / Views / {kontroler} / {akcja}.cshtml
	// Wspó³dzielony widok strony Razor: / Views / Shared / {akcja}.cshtml
	// Wspó³dzielona strona Razor: / Pages / Shared / {akcja}.cshtml
	[Route("prywatnosc")] // ten atrybut spowoduje skrócenie œcie¿ki z domyœlnej https://localhost:5001/home/privacy na https://localhost:5001/prywatnosc
	public IActionResult Privacy()
	{
		return View();
	}

	[Authorize(Roles = "Administratorzy")]
	public IActionResult AdminPrivacy()
	{
		return View("Privacy");
	}

	// Metoda akcji Error przekazuje model widoku do swojego widoku, podaj¹c równie¿ identyfikator u¿ywany do œledzenia.
	// Uzyskana odpowiedŸ nie jest zapisywana w pamiêci podrêcznej.
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}

	// Przedstawiona metoda korzysta z funkcji ASP.NET Core nazywanej wi¹zaniem modelu,
	// aby automatycznie dopasowaæ parametr id przekazany w œcie¿ce do parametru o nazwie id zdefiniowanego w sygnaturze metody.
	public async Task<IActionResult> ProductData(int? id)
	{
		if (!id.HasValue)
		{
			return BadRequest("Musisz podaæ w œcie¿ce ID produktu, na przyk³ad: /Home/DaneProduktu/21");
		}

		Product? model = await _northwindContext.Products.SingleOrDefaultAsync(p => p.ProductId == id);

		return (model == null) ? NotFound($"Nie znaleziono produktu o identyfikatorze {id}.") : View(model);
		//if (model == null)
		//{
		//	return NotFound($"Nie znaleziono produktu o identyfikatorze {id}.");
		//}
		//return View(model); // przekazanie modelu do widoku
	}

	// domyœlnie u¿ywana do obs³u¿enia wszystkich pozosta³ych typów ¿¹dañ HTTP, takich jak GET, PUT, DELETE itd.
	public IActionResult ModelBinding()
	{
		return View(); // strona z formularzem do przes³ania
	}

	[HttpPost]
	public IActionResult ModelBinding(Item item)
	{
		//return View(item); // poka¿ model zwi¹zany z rzecz¹

		HomeModelBindingViewModel model = new(
			item, 
			!ModelState.IsValid, 
			ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage)
		);

		return View(model);
	}

	public IActionResult ProductsThatCostMoreThan(decimal? price)
	{
		if (!price.HasValue)
		{
			return BadRequest("Cenê produktu musisz podaæ w ramach zapytania! Na przyk³ad: /Home/ProductsThatCostMoreThan?price=50");
		}

		IEnumerable<Product> model = _northwindContext.Products
		  .Include(p => p.Category)
		  .Include(p => p.Supplier)
		  .Where(p => p.UnitPrice > price);

		if (!model.Any())
		{
			return NotFound($"Nie ma produktów dro¿szych ni¿ {price:C}.");
		}

		ViewData["MaxPrice"] = price.Value.ToString("C");
		return View(model); // pass model to view
	}

	// Wywo³uje serwis Northwind i pobiera z niego listê klientów, a potem przekazuje j¹ do widoku
	public async Task<IActionResult> Customers(string country)
	{
		string uri;

		if (string.IsNullOrEmpty(country))
		{
			ViewData["Title"] = "Klienci z ca³ego œwiata";
			uri = "api/customers/";
		}
		else
		{
			ViewData["Title"] = $"Klienci z kraju {country}";
			uri = $"api/customers/?country={country}";
		}

		HttpClient client = _httpClientFactory.CreateClient(name: "Northwind.WebApi");
		HttpRequestMessage request = new(method: HttpMethod.Get, requestUri: uri);
		HttpResponseMessage response = await client.SendAsync(request);
		IEnumerable<Customer>? model = await response.Content.ReadFromJsonAsync<IEnumerable<Customer>>();
		return View(model);
	}
}
