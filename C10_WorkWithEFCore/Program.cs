using C10_WorkWithEFCore;
using Microsoft.EntityFrameworkCore; // Include
using Microsoft.EntityFrameworkCore.ChangeTracking; // CollectionEntry
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;  // IDbContextTransaction
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static System.Console;

WriteLine($"Używam dostawcy danych {ProjectConstants.DataProvider}.");

//ZapytanieOKategorie();
//FiltrowanieDolaczen();
//ZapytanieOProdukty();
//ZapytanieZLike();

if (AddProduct(6, "Burgery Boba", 800M))
{
	WriteLine("Dodano nowy produkt.");
}

if (AddProduct(6, "Burgery Boba 2", 700M))
{
	WriteLine("Dodano nowy produkt.");
}

if (AddProduct(6, "Burgery Boba 3", 600M))
{
	WriteLine("Dodano nowy produkt.");
}

if (IncreaseProductPrice(startOfName: "Burg", amount: 20M))
{
	WriteLine("Zaktualizowano cenę produktu.");
}

WriteProducts();

int usuniete = RemoveProducts("Burg");
WriteLine($"{usuniete} produktów zostało usuniętych.");

WriteProducts();

static void CategoryQuery()
{
	using (Northwind db = new())
	{
		var logger = db.GetService<ILoggerFactory>();
		logger.AddProvider(new LoggerProvider());

		WriteLine("Lista kategorii i liczba przypisanych im produktów:");

		// zapytanie pobiera wszystkie kategorie i związane z nimi produkty
		//IQueryable<Category>? categories = db.Categories?
		//	.Include(c => c.Products);

		IQueryable<Category>? categories;
		// = db.Categories;
		//.Include(c => c.Products);

		db.ChangeTracker.LazyLoadingEnabled = false;

		Write("Włączyć ładowanie chętne? (T/N): ");
		bool eagerLoading = (ReadKey().Key == ConsoleKey.T);
		bool explicitLoading = false;
		WriteLine();

		if (eagerLoading)
		{
			categories = db.Categories.Include(c => c.Products);
		}
		else
		{
			categories = db.Categories;

			Write("Włączyć ładowanie jawne? (T/N): ");
			explicitLoading = (ReadKey().Key == ConsoleKey.T);
			WriteLine();
		}

		if (categories is null)
		{
			WriteLine("Nie znaleziono żadnych kategorii.");
			return;
		}

		// wykonaj zapytanie i przejrzyj wyniki
		foreach (Category category in categories)
		{
			if (explicitLoading)
			{
				Write($"Jawnie załadować produkty z kategorii {category.CategoryName}? (T/N):");
				ConsoleKeyInfo key = ReadKey();
				WriteLine();
				if (key.Key == ConsoleKey.T)
				{
					CollectionEntry<Category, Product> products = db.Entry(category).Collection(c => c.Products);
					if (!products.IsLoaded)
					{
						products.Load();
					}
				}
			}

			WriteLine($"Kategoria {category.CategoryName} ma {category.Products.Count} produktów.");
		}
	}
}

static void FilteringInclude()
{
	using (Northwind db = new())
	{
		Write("Podaj minimalną liczbę sztuk w magazynie: ");
		int piecesInStock = int.Parse(ReadLine() ?? "10");

		IQueryable<Category>? categories = db.Categories?
		  .Include(c => c.Products.Where(p => p.UnitsInStock >= piecesInStock));

		if (categories is null)
		{
			WriteLine("Nie znaleziono kategorii.");
			return;
		}

		WriteLine($"ToQueryString: {categories.ToQueryString()}");

		foreach (Category category in categories)
		{
			WriteLine($"Kategoria {category.CategoryName} ma {category.Products.Count} produktów z przynajmniej {piecesInStock} sztukami w magazynie.");
			foreach (Product product in category.Products)
			{
				WriteLine($" Produkt {product.ProductName}: {product.UnitsInStock} sztuk");
			}
		}
	}
}

static void ProductsQuery()
{
	using (Northwind db = new())
	{
		var logger = db.GetService<ILoggerFactory>();
		logger.AddProvider(new LoggerProvider());

		WriteLine("Produkty kosztujące więcej niż podana cena; posortowane:");
		string? input;
		decimal price;

		do
		{
			Write("Podaj cenę produktu: ");
			input = ReadLine();
		} while (!decimal.TryParse(input, out price));

		IQueryable<Product>? products = db.Products?
			.TagWith("Produkty filtrowane według ceny i sortowane.") // Protokołowanie z wykorzystaniem znaczników zapytań
			.Where(p => p.Price > price)
			.OrderByDescending(p => p.Price);

		if (products is null)
		{
			WriteLine("Nie znaleziono produktów.");
			return;
		}

		foreach (Product product in products)
		{
			WriteLine($"{product.ProductID}: {product.ProductName} kosztuje {product.Price:$#,##0.00}. W magazynie jest {product.UnitsInStock} sztuk.");
		}
	}
}

static void QueryWithLike()
{
	using (Northwind db = new())
	{
		ILoggerFactory logger = db.GetService<ILoggerFactory>();
		logger.AddProvider(new LoggerProvider());

		Write("Wprowadź część nazwy produktu: ");
		string? input = ReadLine();

		IQueryable<Product>? products = db.Products?
		   .Where(p => EF.Functions.Like(p.ProductName, $"%{input}%"));

		if (products is null)
		{
			WriteLine("Nie znaleziono produktów.");
			return;
		}

		foreach (Product product in products)
		{
			WriteLine("{0}: w magazynie jest {1} sztuk. Produkt nie jest już wytwarzany? {2}",
			   product.ProductName, product.UnitsInStock, product.Discontinued);
		}
	}
}

static bool AddProduct(int categoryId, string productName, decimal? price)
{
	using (Northwind db = new())
	{
		Product newProduct = new()
		{
			CategoryID = categoryId,
			ProductName = productName,
			Price = price
		};

		// oznacz produkt jako dodany w systemie śledzenia zmian
		db.Products.Add(newProduct);

		// zapisz wszystkie zmiany w bazie
		int changed = db.SaveChanges();
		return (changed == 1);
	}
}

static bool IncreaseProductPrice(string startOfName, decimal amount)
{
	using (var db = new Northwind())
	{
		// pobierz pierwszy produkt, którego nazwa zaczyna się od wartości parametru nazwa
		Product productToEdit = db.Products.First(p => p.ProductName.StartsWith(startOfName));

		productToEdit.Price += amount;

		int changed = db.SaveChanges();
		return (changed == 1);
	}
}

static int RemoveProducts(string startOfName)
{
	using (Northwind db = new())
	{
		using (IDbContextTransaction transaction = db.Database.BeginTransaction())
		{
			WriteLine("Transakcja uruchomiona z poziomem izolacji: {0}",
			   arg0: transaction.GetDbTransaction().IsolationLevel);

			IQueryable<Product>? products = db.Products?.Where(p => p.ProductName.StartsWith(startOfName));

			if (products is null)
			{
				WriteLine("Nie znaleziono produktów do usunięcia.");
				return 0;
			}
			else
			{
				db.Products.RemoveRange(products);
			}

			int changed = db.SaveChanges();
			transaction.Commit();
			return changed;
		}
	}
}

static void WriteProducts()
{
	using (Northwind db = new())
	{
		WriteLine("{0,-3} {1,-35} {2,8} {3,5} {4}",
			"ID", "Nazwa", "Koszt", "Stan", "Nieprod.");
		// ,-35 = wyrównanie do lewej wartości parametru w ramach kolumny o szerokości 35 znaków.
		// ,5 = wyrównanie do prawej wartości parametru w ramach kolumny o szerokości 5 znaków.

		foreach (Product product in db.Products.OrderByDescending(p => p.Price))
		{
			WriteLine("{0:000} {1,-35} {2,8:$#,##0.00} {3,5} {4}",
			   product.ProductID, product.ProductName, product.Price,
			   product.UnitsInStock, product.Discontinued);
		}
	}
}