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
	// Deklarowane jest prywatne pole przechowuj�ce referencj� obiektu protoko�uj�cego, kt�ry jest przekazywany kontrolerowi HomeController w konstruktorze.
	private readonly ILogger<HomeController> _logger;

	private readonly NorthwindContext _northwindContext;
	private readonly IHttpClientFactory _httpClientFactory;

	// Okre�lenie w konstruktorze klasy serwis�w wymaganych przez kontroler w celu osi�gni�cia poprawnego stanu, kt�ry umo�liwia mu normaln� prac�.
	public HomeController(ILogger<HomeController> logger, NorthwindContext context, IHttpClientFactory clientFactory)
	{
		_logger = logger;
		_northwindContext = context;
		_httpClientFactory = clientFactory;
	}

	// Wszystkie trzy metody akcji wywo�uj� metod� View() i zwracaj� otrzymany od niej wynik typu IActionResult jako odpowied� dla klienta.
	public async Task<IActionResult> Index()
	{
		_logger.LogError("Naprawd� powa�ny b��d (tylko �artuj�!)");
		_logger.LogWarning("To jest pierwsze ostrze�enie!");
		_logger.LogWarning("Drugie ostrze�enie!");
		_logger.LogInformation("Jestem metod� Index kontrolera HomeController.");

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

	// Mo�emy te� chcie� zapisywa� w pami�ci podr�cznej odpowiedzi HTTP generowane przez akcj� modelu. W tym celu nale�y dopisa� do metody atrybut [ResponseCache].
	// Mo�emy zdecydowa�, gdzie i jak d�ugo odpowied� ma by� przechowywana w pami�ci podr�cznej, stosuj�c do tego parametry podane na poni�szej li�cie:
	// Duration � warto�� podawana w sekundach.Ustala warto�� nag��wka HTTP o nazwie max-age.
	// Location � jedna z warto�ci typu ResponseCacheLocation: Any, Client lub None.Ustala warto�� nag��wka HTTP o nazwie cache-control.
	// NoStore � je�eli ma warto�� true, to parametry Duration i Location s� ignorowane, a nag��wkowi HTTP o nazwie cache-control przypisywana jest warto�� no-store.
	// Oznacza to, �e przez podany czas przegl�darka przy ka�dym wej�ciu b�dzie u�ywa�a zapami�tanej strony zamis �adowa� j� od nowa
	[ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
	public IActionResult IndexWithCache()
	{
		return View("Index");
	}

	// Metody Index i Privacy maj� jednakow� implementacj�, a mimo to zwracaj� odmienne strony WWW. Wynika to z zastosowania konwencji.
	// W obu przypadkach metoda View poszukuje plik�w Razor u�ywanych do generowania strony WWW, ale szuka ich w innych �cie�kach:
	// Konkretny widok strony Razor: / Views / {kontroler} / {akcja}.cshtml
	// Wsp�dzielony widok strony Razor: / Views / Shared / {akcja}.cshtml
	// Wsp�dzielona strona Razor: / Pages / Shared / {akcja}.cshtml
	[Route("prywatnosc")] // ten atrybut spowoduje skr�cenie �cie�ki z domy�lnej https://localhost:5001/home/privacy na https://localhost:5001/prywatnosc
	public IActionResult Privacy()
	{
		return View();
	}

	[Authorize(Roles = "Administratorzy")]
	public IActionResult AdminPrivacy()
	{
		return View("Privacy");
	}

	// Metoda akcji Error przekazuje model widoku do swojego widoku, podaj�c r�wnie� identyfikator u�ywany do �ledzenia.
	// Uzyskana odpowied� nie jest zapisywana w pami�ci podr�cznej.
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}

	// Przedstawiona metoda korzysta z funkcji ASP.NET Core nazywanej wi�zaniem modelu,
	// aby automatycznie dopasowa� parametr id przekazany w �cie�ce do parametru o nazwie id zdefiniowanego w sygnaturze metody.
	public async Task<IActionResult> ProductData(int? id)
	{
		if (!id.HasValue)
		{
			return BadRequest("Musisz poda� w �cie�ce ID produktu, na przyk�ad: /Home/DaneProduktu/21");
		}

		Product? model = await _northwindContext.Products.SingleOrDefaultAsync(p => p.ProductId == id);

		return (model == null) ? NotFound($"Nie znaleziono produktu o identyfikatorze {id}.") : View(model);
		//if (model == null)
		//{
		//	return NotFound($"Nie znaleziono produktu o identyfikatorze {id}.");
		//}
		//return View(model); // przekazanie modelu do widoku
	}

	// domy�lnie u�ywana do obs�u�enia wszystkich pozosta�ych typ�w ��da� HTTP, takich jak GET, PUT, DELETE itd.
	public IActionResult ModelBinding()
	{
		return View(); // strona z formularzem do przes�ania
	}

	[HttpPost]
	public IActionResult ModelBinding(Item item)
	{
		//return View(item); // poka� model zwi�zany z rzecz�

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
			return BadRequest("Cen� produktu musisz poda� w ramach zapytania! Na przyk�ad: /Home/ProductsThatCostMoreThan?price=50");
		}

		IEnumerable<Product> model = _northwindContext.Products
		  .Include(p => p.Category)
		  .Include(p => p.Supplier)
		  .Where(p => p.UnitPrice > price);

		if (!model.Any())
		{
			return NotFound($"Nie ma produkt�w dro�szych ni� {price:C}.");
		}

		ViewData["MaxPrice"] = price.Value.ToString("C");
		return View(model); // pass model to view
	}

	// Wywo�uje serwis Northwind i pobiera z niego list� klient�w, a potem przekazuje j� do widoku
	public async Task<IActionResult> Customers(string country)
	{
		string uri;

		if (string.IsNullOrEmpty(country))
		{
			ViewData["Title"] = "Klienci z ca�ego �wiata";
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
