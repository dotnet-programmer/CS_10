using BibliotekaWspolna;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using static System.Console;

FiltrujISortuj();
Console.WriteLine();
FiltrujISortuj2();
Console.WriteLine();
ZlaczKategorieIProdukty();
Console.WriteLine();
GrupujIZlaczKategorieIProdukty();
Console.WriteLine();
AgregowanieTabeliProducts();
Console.WriteLine();
WlasneMetodyRozszerzajace();
Console.WriteLine();
WypiszProduktyJakoXml();
Console.WriteLine();
LadujUstawienia();

static void FiltrujISortuj()
{
	using (Northwind bd = new())
	{
		DbSet<Product> wszystkieProdukty = bd.Products;

		IQueryable<Product> filtrowaneProdukty = wszystkieProdukty.Where(produkt => produkt.UnitPrice < 10M);

		IOrderedQueryable<Product> sortowaneFiltrowaneProdukty = filtrowaneProdukty.OrderByDescending(produkt => produkt.UnitPrice);

		WriteLine($"ToQueryString: {sortowaneFiltrowaneProdukty.ToQueryString()}");

		WriteLine("Produkty kosztujące mniej niż 10$:");
		foreach (Product pozycja in sortowaneFiltrowaneProdukty)
		{
			WriteLine("{0}: {1} kosztuje {2:$#,##0.00}",
			  arg0: pozycja.ProductID,
			  arg1: pozycja.ProductName,
			  arg2: pozycja.UnitPrice);
		}
		WriteLine();
	}
}

static void FiltrujISortuj2()
{
	using (Northwind bd = new())
	{
		var producsFromDB = bd.Products
			.Where(produkt => produkt.UnitPrice < 10M)
			.OrderByDescending(produkt => produkt.UnitPrice)
			.Select(produkt => new
			{
				produkt.ProductID,
				produkt.ProductName,
				produkt.UnitPrice
			});

		WriteLine($"ToQueryString: {producsFromDB.ToQueryString()}");

		WriteLine("Produkty kosztujące mniej niż 10$:");
		foreach (var pozycja in producsFromDB)
		{
			WriteLine("{0}: {1} kosztuje {2:$#,##0.00}",
			  arg0: pozycja.ProductID,
			  arg1: pozycja.ProductName,
			  arg2: pozycja.UnitPrice);
		}
		WriteLine();
	}
}

static void FiltrujISortuj3()
{
	using (Northwind bd = new())
	{
		DbSet<Product> wszystkieProdukty = bd.Products;

		if (wszystkieProdukty is null)
		{
			WriteLine("Nie znaleziono produktów.");
			return;
		}

		IQueryable<Product> przetworzoneProdukty = wszystkieProdukty.PrzetwarzajSekwencje();

		IQueryable<Product> filtrowaneProdukty = wszystkieProdukty.Where(produkt => produkt.UnitPrice < 10M);

		IOrderedQueryable<Product> sortowaneFiltrowaneProdukty = filtrowaneProdukty.OrderByDescending(produkt => produkt.UnitPrice);

		WriteLine($"ToQueryString: {sortowaneFiltrowaneProdukty.ToQueryString()}");

		WriteLine("Produkty kosztujące mniej niż 10$:");
		foreach (Product pozycja in sortowaneFiltrowaneProdukty)
		{
			WriteLine("{0}: {1} kosztuje {2:$#,##0.00}",
			  arg0: pozycja.ProductID,
			  arg1: pozycja.ProductName,
			  arg2: pozycja.UnitPrice);
		}
		WriteLine();
	}
}

static void ZlaczKategorieIProdukty()
{
	using (Northwind db = new())
	{
		// złącz każdy produkt z odpowiednią kategorią i zwróć 77 dopasowań
		var zapytanieJoin = db.Categories.Join(
		   inner: db.Products,
		   outerKeySelector: kategoria => kategoria.CategoryID,
		   innerKeySelector: produkt => produkt.CategoryID,
		   resultSelector: (k, p) => new { k.CategoryName, p.ProductName, p.ProductID })
			.OrderBy(kat => kat.CategoryName);

		WriteLine($"ToQueryString: {zapytanieJoin.ToQueryString()}");

		foreach (var wynik in zapytanieJoin)
		{
			WriteLine("{0}: {1} w kategorii {2}.",
			   arg0: wynik.ProductID,
			   arg1: wynik.ProductName,
			   arg2: wynik.CategoryName);
		}
	}
}

static void GrupujIZlaczKategorieIProdukty()
{
	using (Northwind db = new())
	{
		// zgrupuj wszystkie produkty według kategorii i wypisz 8 grup
		var zapytanieGrupujace = db.Categories.AsEnumerable().GroupJoin(
		  inner: db.Products,
		  outerKeySelector: kategoria => kategoria.CategoryID,
		  innerKeySelector: produkt => produkt.CategoryID,
		  resultSelector: (k, pasujaceProdukty) => new
		  {
			  k.CategoryName,
			  Products = pasujaceProdukty.OrderBy(p => p.ProductName)
		  });

		foreach (var element in zapytanieGrupujace)
		{
			WriteLine("Kategoria {0} ma {1} produktów.",
			  arg0: element.CategoryName,
			  arg1: element.Products.Count());

			foreach (var produkt in element.Products)
			{
				WriteLine($" {produkt.ProductName}");
			}
		}
	}
}

static void AgregowanieTabeliProducts()
{
	using (var db = new Northwind())
	{
		WriteLine("{0,-30} {1,10}",
		   arg0: "Liczba produktów:",
		   arg1: db.Products.Count());

		WriteLine("{0,-30} {1,10:$#,##0.00}",
		   arg0: "Najwyższa cena produktu:",
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

static void WlasneMetodyRozszerzajace()
{
	using (Northwind db = new())
	{
		WriteLine("Średnia liczby jednostek w magazynie: {0:N0}",
		  db.Products.Average(p => p.UnitsInStock));

		WriteLine("Średnia cena jednostki:{0:$#,##0.00}",
		  db.Products.Average(p => p.UnitPrice));

		WriteLine("Mediana liczby jednostek w magazynie:{0:N0}",
		  db.Products.Mediana(p => p.UnitsInStock));

		WriteLine("Mediana ceny jednostek:{0:$#,##0.00}",
		  db.Products.Mediana(p => p.UnitPrice));

		WriteLine("Dominanta liczby jednostek w magazynie:{0:N0}",
		  db.Products.Dominanta(p => p.UnitsInStock));

		WriteLine("Dominanta ceny jednostek:{0:$#,##0.00}",
		  db.Products.Dominanta(p => p.UnitPrice));
	}
}

static void WypiszProduktyJakoXml()
{
	using (Northwind db = new())
	{
		Product[] tablicaProduktow = db.Products.ToArray();

		XElement xml = new("produkty",
		  from p in tablicaProduktow
		  select new XElement("produkt",
			new XAttribute("id", p.ProductID),
			new XAttribute("cena", p.UnitPrice),
		   new XElement("nazwa", p.ProductName)));

		WriteLine(xml.ToString());
	}
}

static void LadujUstawienia()
{
	XDocument dokument = XDocument.Load("ustawienia.xml");

	var ustawieniaAplikacji = dokument.Descendants("appSettings")
	  .Descendants("add")
	  .Select(wezel => new
	  {
		  Key = wezel.Attribute("key")?.Value,
		  Value = wezel.Attribute("value")?.Value
	  }).ToArray();

	foreach (var element in ustawieniaAplikacji)
	{
		WriteLine($"{element.Key}: {element.Value}");
	}
}