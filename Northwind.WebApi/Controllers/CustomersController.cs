using CommonLibrary;  // Customer
using Microsoft.AspNetCore.Mvc;  // [Route], [ApiController], ControllerBase
using Northwind.WebApi.Reposiories;  // ICustomerRepository

namespace Northwind.WebApi.Controllers;

// Gdy serwis odbiera żądanie HTTP, najpierw tworzy obiekt klasy kontrolera i wywołuje w nim odpowiednią metodę akcji.
// Następnie zwraca klientowi odpowiedź w wybranym przez niego formacie i zwalnia zasoby zajmowane przez kontroler,
// w tym też repozytorium oraz kontekst danych.

// adres bazowy: api/customers
[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
	private readonly ICustomerRepository _customerRepository;

	// konstruktor wstrzykuje zarejestrowane repozytorium
	public CustomersController(ICustomerRepository repo)
		=> _customerRepository = repo;

	// GET: api/customers
	// GET: api/customers/?country=[country]
	// Zawsze zwraca listę klientów, nawet jeżeli ta jest pusta.
	[HttpGet]
	[ProducesResponseType(200, Type = typeof(IEnumerable<Customer>))]
	public async Task<IEnumerable<Customer>> ReadCustomers(string? country)
	{
		if (string.IsNullOrWhiteSpace(country))
		{
			return await _customerRepository.ReadAllAsync();
		}
		else
		{
			return (await _customerRepository.ReadAllAsync()).Where(client => client.Country == country);
		}
	}

	// GET: api/customers/[id]
	[HttpGet("{id}", Name = "ReadCustomer")]  // nazwana ścieżka
	[ProducesResponseType(200, Type = typeof(Customer))]
	[ProducesResponseType(404)]
	public async Task<IActionResult> ReadCustomer(string id)
	{
		Customer? customer = await _customerRepository.ReadAsync(id);
		return customer switch
		{
			null => NotFound(), // 404 Nie znaleziono
			_ => Ok(customer) // 200 OK z danymi klienta w ciele odpowiedzi
		};
	}

	// POST: api/customers
	// BODY: Customer (JSON, XML)
	[HttpPost]
	[ProducesResponseType(201, Type = typeof(Customer))]
	[ProducesResponseType(400)]
	// atrybut [FromBody] informuje mechanizm wiązania modelu, że ten ma wypełnić model danymi z treści żądania POST.
	public async Task<IActionResult> Create([FromBody] Customer customer)
	{
		if (customer == null)
		{
			return BadRequest(); // 400 Błąd żądania
		}

		Customer? addedCustomer = await _customerRepository.CreateAsync(customer);

		if (addedCustomer == null)
		{
			return BadRequest("Repozytorium nie utworzyło klienta");
		}
		else
		{
			// Metoda Utworz zwraca odpowiedź, wykorzystując przy tym ścieżkę OdczytajKlienta,
			// dzięki czemu klient od razu wie, jak pobrać nowo utworzony zasób.
			// Dopasowujemy do siebie poszczególne metody, aby utworzyć klienta, a następnie pobrać jego dane.
			return CreatedAtRoute( // 201 Utworzono
			  routeName: nameof(ReadCustomer),
			  routeValues: new { id = addedCustomer.CustomerId.ToLower() },
			  value: addedCustomer);
		}
	}

	// PUT: api/customers/[id]
	// BODY: Customer (JSON, XML)
	[HttpPut("{id}")]
	[ProducesResponseType(204)]
	[ProducesResponseType(400)]
	[ProducesResponseType(404)]
	public async Task<IActionResult> Update(string id, [FromBody] Customer customer)
	{
		id = id.ToUpper();
		customer.CustomerId = customer.CustomerId.ToUpper();

		if (customer == null || customer.CustomerId != id)
		{
			return BadRequest(); // 400 Błąd żądania
		}

		Customer? existing = await _customerRepository.ReadAsync(id);
		if (existing == null)
		{
			return NotFound(); // 404 Nie znaleziono zasobu
		}

		await _customerRepository.UpdateAsync(id, customer);

		return new NoContentResult(); // 204 Bez treści
	}

	// DELETE: api/customers/[id]
	[HttpDelete("{id}")]
	[ProducesResponseType(204)]
	[ProducesResponseType(400)]
	[ProducesResponseType(404)]
	public async Task<IActionResult> Delete(string id)
	{
		// przygotuj szczegółowe dane problemu
		if (id == "zle")
		{
			var problemDetails = new ProblemDetails
			{
				Status = StatusCodes.Status400BadRequest,
				Type = "https://localhost:5001/customers/cannot-be-deleted",
				Title = $"Klient {id} został znaleziony, ale nie udało się go usunąć.",
				Detail = "dodatkowe szczegóły, takie jak nazwa firmy, kraj itp. ",
				Instance = HttpContext.Request.Path
			};
			return BadRequest(problemDetails); // 400 Błąd żądania
		}

		Customer? existing = await _customerRepository.ReadAsync(id);
		if (existing == null)
		{
			return NotFound(); // 404 Nie znaleziono
		}

		bool? deleted = await _customerRepository.DeleteAsync(id);

		if (deleted.HasValue && deleted.Value) // skrócony iloczyn logiczny
		{
			return new NoContentResult(); // 204 Bez treści
		}
		else
		{
			// 400 Błąd żądania
			return BadRequest($"Klient {id} został znaleziony, ale nie udało się go usunąć.");
		}
	}
}