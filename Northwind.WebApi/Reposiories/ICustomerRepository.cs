using CommonLibrary;

namespace Northwind.WebApi.Reposiories;

public interface ICustomerRepository
{
	Task<Customer?> CreateAsync(Customer customer);
	Task<IEnumerable<Customer>> ReadAllAsync();
	Task<Customer?> ReadAsync(string id);
	Task<Customer?> UpdateAsync(string id, Customer customer);
	Task<bool?> DeleteAsync(string id);
}