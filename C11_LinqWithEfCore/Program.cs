using System.Xml.Linq;
using C11_LinqWithEfCore;
using Microsoft.EntityFrameworkCore;
using static System.Console;

FilterAndSort();
WriteLine();
FilterAndSort2();
WriteLine();
CombineCategoriesAndProducts();
WriteLine();
GroupAndCombineCategoriesAndProducts();
WriteLine();
AggregatingProductsTable();
WriteLine();
OwnExtensionMethods();
WriteLine();
ListProductsAsXml();
WriteLine();
LoadSettings();

static void FilterAndSort()
{
	using (Northwind bd = new())
	{
		DbSet<Product> allProducts = bd.Products;

		IQueryable<Product> filteredProducts = allProducts.Where(product => product.UnitPrice < 10M);

		IOrderedQueryable<Product> sortedFilteredProducts = filteredProducts.OrderByDescending(product => product.UnitPrice);

		WriteLine($"ToQueryString: {sortedFilteredProducts.ToQueryString()}");

		WriteLine("Produkty kosztujące mniej niż 10$:");
		foreach (var item in sortedFilteredProducts)
		{
			WriteLine($"{item.ProductID}: {item.ProductName} kosztuje {item.UnitPrice:$#,##0.00}");
		}
		WriteLine();
	}
}

static void FilterAndSort2()
{
	using (Northwind bd = new())
	{
		var producsFromDB = bd.Products
			.Where(product => product.UnitPrice < 10M)
			.OrderByDescending(product => product.UnitPrice)
			.Select(product => new
			{
				product.ProductID,
				product.ProductName,
				product.UnitPrice
			});

		WriteLine($"ToQueryString: {producsFromDB.ToQueryString()}");

		WriteLine("Produkty kosztujące mniej niż 10$:");
		foreach (var item in producsFromDB)
		{
			WriteLine($"{item.ProductID}: {item.ProductName} kosztuje {item.UnitPrice:$#,##0.00}");
		}
		WriteLine();
	}
}

static void FilterAndSort3()
{
	using (Northwind bd = new())
	{
		DbSet<Product> allProducts = bd.Products;

		if (allProducts is null)
		{
			WriteLine("Nie znaleziono productów.");
			return;
		}

		IQueryable<Product> processedProducts = allProducts.ProcessSequences();

		IQueryable<Product> filteredProducts = allProducts.Where(product => product.UnitPrice < 10M);

		IOrderedQueryable<Product> sortedFilteredProducts = filteredProducts.OrderByDescending(product => product.UnitPrice);

		WriteLine($"ToQueryString: {sortedFilteredProducts.ToQueryString()}");

		WriteLine("Produkty kosztujące mniej niż 10$:");
		foreach (var item in sortedFilteredProducts)
		{
			WriteLine($"{item.ProductID}: {item.ProductName} kosztuje {item.UnitPrice:$#,##0.00}");
		}
		WriteLine();
	}
}

static void CombineCategoriesAndProducts()
{
	using (Northwind db = new())
	{
		// złącz każdy product z odpowiednią kategorią i zwróć 77 dopasowań
		var joinQuery = db.Categories.Join(
		   inner: db.Products,
		   outerKeySelector: category => category.CategoryID,
		   innerKeySelector: product => product.CategoryID,
		   resultSelector: (c, p) => new { c.CategoryName, p.ProductName, p.ProductID })
			.OrderBy(c => c.CategoryName);

		WriteLine($"ToQueryString: {joinQuery.ToQueryString()}");

		foreach (var item in joinQuery)
		{
			WriteLine($"{item.ProductID}: {item.ProductName} w kategorii {item.CategoryName}.");
		}
	}
}

static void GroupAndCombineCategoriesAndProducts()
{
	using (Northwind db = new())
	{
		// zgrupuj wszystkie producty według kategorii i wypisz 8 grup
		var groupingQuery = db.Categories.AsEnumerable().GroupJoin(
		  inner: db.Products,
		  outerKeySelector: category => category.CategoryID,
		  innerKeySelector: product => product.CategoryID,
		  resultSelector: (c, matchingProducts) => new
		  {
			  c.CategoryName,
			  Products = matchingProducts.OrderBy(p => p.ProductName)
		  });

		foreach (var item in groupingQuery)
		{
			WriteLine($"Kategoria {item.CategoryName} ma {item.Products.Count()} productów.");

			foreach (var product in item.Products)
			{
				WriteLine($" {product.ProductName}");
			}
		}
	}
}

static void AggregatingProductsTable()
{
	using (var db = new Northwind())
	{
		WriteLine("{0,-30} {1,10}",
		   arg0: "Liczba productów:",
		   arg1: db.Products.Count());

		WriteLine("{0,-30} {1,10:$#,##0.00}",
		   arg0: "Najwyższa cena productu:",
		   arg1: db.Products.Max(p => p.UnitPrice));

		WriteLine("{0,-30} {1,10:N0}",
		   arg0: "Suma jednostek w magazynie:",
		   arg1: db.Products.Sum(p => p.UnitsInStock));

		WriteLine("{0,-30} {1,10:N0}",
		   arg0: "Suma jednostek w zamówieniu:",
		   arg1: db.Products.Sum(p => p.UnitsOnOrder));

		WriteLine("{0,-30} {1,10:$#,##0.00}",
		   arg0: "Średnia cena jednostki:",
		   arg1: db.Products.Average(p => p.UnitPrice));

		WriteLine("{0,-30} {1,10:$#,##0.00}",
		   arg0: "Wartość jednostek w magazynie:",
		   arg1: db.Products.AsEnumerable()
			  .Sum(p => p.UnitPrice * p.UnitsInStock));
	}
}

static void OwnExtensionMethods()
{
	using (Northwind db = new())
	{
		DbSet<Product>? allProducts = db.Products;
		if (allProducts is null)
		{
			WriteLine("Nie znaleziono productów.");
			return;
		}
		IQueryable<Product> processedProducts = allProducts.ProcessSequences();
		IQueryable<Product> filteredProducts = processedProducts.Where(product => product.UnitPrice < 10M);
		WriteLine("Produkty kosztujące mniej niż 10$:");
		foreach (var item in filteredProducts)
		{
			WriteLine($"{item.ProductID}: {item.ProductName} kosztuje {item.UnitPrice:$#,##0.00}");
		}
		WriteLine();

		WriteLine("Średnia liczby jednostek w magazynie: {0:N0}",
		  db.Products.Average(p => p.UnitsInStock));

		WriteLine("Średnia cena jednostki:{0:$#,##0.00}",
		  db.Products.Average(p => p.UnitPrice));

		WriteLine("Mediana liczby jednostek w magazynie:{0:N0}",
		  db.Products.Median(p => p.UnitsInStock));

		WriteLine("Mediana ceny jednostek:{0:$#,##0.00}",
		  db.Products.Median(p => p.UnitPrice));

		WriteLine("Dominanta liczby jednostek w magazynie:{0:N0}",
		  db.Products.Dominant(p => p.UnitsInStock));

		WriteLine("Dominanta ceny jednostek:{0:$#,##0.00}",
		  db.Products.Dominant(p => p.UnitPrice));
	}
}

static void ListProductsAsXml()
{
	using (Northwind db = new())
	{
		Product[] productTable = db.Products.ToArray();

		XElement xml = new("produkty",
		  from p in productTable
		  select new XElement("product",
			new XAttribute("id", p.ProductID),
			new XAttribute("cena", p.UnitPrice),
		   new XElement("nazwa", p.ProductName)));

		WriteLine(xml.ToString());
	}
}

//załaduje plik XML;
//poszuka w załadowanych danych elementu o nazwie appSettings oraz znajdujących się w nim elementów add;
//przeniesie dane XML do tablicy typów anonimowych z właściwościami Key i Value;
//przejrzy w pętli całą tablicę i wypisze w konsoli wyniki.
static void LoadSettings()
{
	XDocument document = XDocument.Load("Settings.xml");

	var appSettings = document
		.Descendants("appSettings")
		.Descendants("add")
		.Select(node => new
		{
			Key = node.Attribute("key")?.Value,
			Value = node.Attribute("value")?.Value
		}).ToArray();

	foreach (var element in appSettings)
	{
		WriteLine($"{element.Key}: {element.Value}");
	}
}