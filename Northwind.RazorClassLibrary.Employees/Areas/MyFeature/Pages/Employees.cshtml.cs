using Microsoft.AspNetCore.Mvc.RazorPages; // PageModel
using CommonLibrary; // Employee, NorthwindContext
 
namespace Northwind.RazorClassLibrary.Employees.MyFeature.Pages;

public class EmployeesPageModel(NorthwindContext northwindContext) : PageModel
{
	private readonly NorthwindContext _northwindContext = northwindContext;

	public IEnumerable<Employee> Employees { get; set; }

	public void OnGet()
	{
		ViewData["Title"] = "Firma Northwind - Pracownicy";

		Employees = _northwindContext
			.Employees
			.OrderBy(e => e.LastName)
			.ThenBy(e => e.FirstName)
			.ToArray();
	}
}