namespace Northwind.Mvc.Models;

public record HomeModelBindingViewModel(
	Item Item, 
	bool HasErrors, 
	IEnumerable<string> ValidationErrors);