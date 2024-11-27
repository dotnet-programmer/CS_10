using System.Collections.Concurrent; // ConcurrentDictionary
using CommonLibrary; // Customer
using Microsoft.EntityFrameworkCore.ChangeTracking;  // EntityEntry<T>

namespace Northwind.WebApi.Reposiories;

public class CustomerRepository : ICustomerRepository
{
	// dane klientów umieszczane są w wielowątkowym słowniku, co znacznie poprawia szybkość pracy
	private static ConcurrentDictionary<string, Customer>? _cacheMemory;

	// Kontekst danych umieszczamy w polu obiektu, ponieważ tutaj używamy własnej pamięci podręcznej.
	private readonly NorthwindContext _context;

	public CustomerRepository(NorthwindContext context)
	{
		_context = context;

		// załaduj dane klientów z bazy danych i umieść je w słowniku, którego kluczem jest ID klienta
		// następnie przekształć słownik w wielowątkowy obiekt ConcurrentDictionary
		if (_cacheMemory == null)
		{
			_cacheMemory = new ConcurrentDictionary<string, Customer>(_context.Customers.ToDictionary(c => c.CustomerId));
		}
	}

	public async Task<Customer?> CreateAsync(Customer customer)
	{
		// normalizowanie ID klienta — tylko wielkie litery
		customer.CustomerId = customer.CustomerId.ToUpper();

		// dodaj do bazy danych, używając EF Core
		EntityEntry<Customer> added = await _context.Customers.AddAsync(customer);
		int changed = await _context.SaveChangesAsync();
		if (changed == 1)
		{
			if (_cacheMemory is null)
			{
				return customer;
			}
			// jeżeli to nowy klient, to dodaj go do pamięci podręcznej,inaczej wywołaj metodę UpdateCache
			return _cacheMemory.AddOrUpdate(customer.CustomerId, customer, UpdateCache);
		}
		else
		{
			return null;
		}
	}

	public Task<IEnumerable<Customer>> ReadAllAsync() =>
		// pobierz z pamięci podręcznej — tak jest szybciej
		Task.FromResult(_cacheMemory is null
			? Enumerable.Empty<Customer>()
			: _cacheMemory.Values);

	public Task<Customer?> ReadAsync(string id)
	{
		// pobierz z pamięci podręcznej — tak jest szybciej
		id = id.ToUpper();
		if (_cacheMemory is null)
		{
			return null!;
		}

		_cacheMemory.TryGetValue(id, out Customer? customer);
		return Task.FromResult(customer);
	}

	public async Task<Customer?> UpdateAsync(string id, Customer customer)
	{
		// normalizowanie id klienta
		id = id.ToUpper();
		customer.CustomerId = customer.CustomerId.ToUpper();

		// zaktualizuj w bazie danych
		_context.Customers.Update(customer);
		int changed = await _context.SaveChangesAsync();
		if (changed == 1)
		{
			// zaktualizuj w pamięci podręcznej
			return UpdateCache(id, customer);
		}
		return null;
	}

	public async Task<bool?> DeleteAsync(string id)
	{
		id = id.ToUpper();

		// usuń z bazy danych
		Customer? customer = _context.Customers.Find(id);
		if (customer is null)
		{
			return null;
		}

		_context.Customers.Remove(customer);
		int changed = await _context.SaveChangesAsync();
		if (changed == 1)
		{
			if (_cacheMemory is null)
			{
				return null;
			}
			// usuń z pamięci podręcznej
			return _cacheMemory.TryRemove(id, out customer);
		}
		else
		{
			return null;
		}
	}

	private Customer UpdateCache(string id, Customer customer)
	{
		if (_cacheMemory is not null
			&& _cacheMemory.TryGetValue(id, out Customer? oldCustomer)
			&& _cacheMemory.TryUpdate(id, customer, oldCustomer))
		{
			return customer;
		}
		return null!;
	}

}