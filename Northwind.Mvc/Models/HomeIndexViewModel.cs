using CommonLibrary;  // Category, Product

namespace Northwind.Mvc.Models;

// Przy nadawaniu nazw klasom modeli widoków powinno się stosować konwencję {Kontroler}{Akcja}ViewModel.
public record HomeIndexViewModel
{
	public int NumberOfVisits;
	public IList<Category> Categories;
	public IList<Product> Products;
}