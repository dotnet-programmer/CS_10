using CommonLibrary; // NorthwindContext, Supplier
using Microsoft.AspNetCore.Mvc;  // [BindProperty], IActionResult
using Microsoft.AspNetCore.Mvc.RazorPages; // PageModel

namespace Northwind.Web.Pages;

public class SuppliersModel(NorthwindContext northwindContext) : PageModel
{
	private readonly NorthwindContext _northwindContext = northwindContext;

	public IEnumerable<Supplier>? Suppliers { get; set; }

	// atrybut[BindProperty], pozwala na ³atwe ³¹czenie elementów jêzyka HTML na stronie z w³aœciwoœciami klasy Supplier
	[BindProperty]
	public Supplier? Supplier { get; set; }

	public void OnGet()
	{
		ViewData["Title"] = "Witryna WWW firmy Northwind - Dostawcy";

		//Suppliers = new[] { "Alpha Co", "Beta Limited", "Gamma Corp" };
		Suppliers = _northwindContext
			.Suppliers
			.OrderBy(s => s.Country)
			.ThenBy(s => s.CompanyName);
	}

	// metoda reaguj¹ca na ¿¹dania HTTP POST
	public IActionResult OnPost()
	{
		if ((Supplier is not null) && ModelState.IsValid)
		{
			_northwindContext.Suppliers.Add(Supplier);
			_northwindContext.SaveChanges();
			return RedirectToPage("/Suppliers");
		}
		return Page();
	}
}
