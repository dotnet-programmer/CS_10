using BibliotekaWspolna;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking; // CollectionEntry
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;  // IDbContextTransaction
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static System.Console;

WriteLine($"Używam dostawcy danych {StaleProjektu.DostawcaDanych}.");

//ZapytanieOKategorie();
//FiltrowanieDolaczen();
//ZapytanieOProdukty();
//ZapytanieZLike();

if (DodajProdukt(6, "Burgery Boba", 800M))
{
	WriteLine("Dodano nowy produkt.");
}

if (DodajProdukt(6, "Burgery Boba 2", 700M))
{
	WriteLine("Dodano nowy produkt.");
}

if (DodajProdukt(6, "Burgery Boba 3", 600M))
{
	WriteLine("Dodano nowy produkt.");
}

if (ZwiekszCeneProduktu(poczatekNazwy: "Burg", kwota: 20M))
{
	WriteLine("Zaktualizowano cenę produktu.");
}

WypiszProdukty();

int usuniete = UsunProdukty("Burg");
WriteLine($"{usuniete} produktów zostało usuniętych.");

WypiszProdukty();

static void ZapytanieOKategorie()
{
	using (Northwind db = new())
	{
		var fabrykaProtokolu = db.GetService<ILoggerFactory>();
		fabrykaProtokolu.AddProvider(new DostawcaProtokoluKonsoli());

		WriteLine("Lista kategorii i liczba przypisanych im produktów:");

		// zapytanie pobiera wszystkie kategorie i związane z nimi produkty
		//IQueryable<Category>? kategorie = db.Categories?
		//	.Include(c => c.Products);

		IQueryable<Category>? kategorie;
		// = db.Categories;
		//.Include(c => c.Products);

		db.ChangeTracker.LazyLoadingEnabled = false;

		Write("Włączyć ładowanie chętne? (T/N): ");
		bool ladowanieChetne = (ReadKey().Key == ConsoleKey.T);
		bool ladowanieJawne = false;
		WriteLine();

		if (ladowanieChetne)
		{
			kategorie = db.Categories.Include(c => c.Products);
		}
		else
		{
			kategorie = db.Categories;

			Write("Włączyć ładowanie jawne? (T/N): ");
			ladowanieJawne = (ReadKey().Key == ConsoleKey.T);
			WriteLine();
		}

		if (kategorie is null)
		{
			WriteLine("Nie znaleziono żadnych kategorii.");
			return;
		}

		// wykonaj zapytanie i przejrzyj wyniki
		foreach (Category k in kategorie)
		{
			if (ladowanieJawne)
			{
				Write($"Jawnie załadować produkty z kategorii {k.CategoryName}? (T/N):");
				ConsoleKeyInfo key = ReadKey();
				WriteLine();
				if (key.Key == ConsoleKey.T)
				{
					CollectionEntry<Category, Product> produkty =
					   db.Entry(k).Collection(c2 => c2.Products);
					if (!produkty.IsLoaded)
					{
						produkty.Load();
					}
				}
			}

			WriteLine($"Kategoria {k.CategoryName} ma {k.Products.Count} produktów.");
		}
	}
}

static void FiltrowanieDolaczen()
{
	using (Northwind db = new())
	{
		Write("Podaj minimalną liczbę sztuk w magazynie: ");
		string sztukWMagazynie = ReadLine() ?? "10";
		int sztuki = int.Parse(sztukWMagazynie);

		IQueryable<Category>? kategorie = db.Categories?
		  .Include(c => c.Products.Where(p => p.WMagazynie >= sztuki));

		if (kategorie is null)
		{
			WriteLine("Nie znaleziono kategorii.");
			return;
		}

		WriteLine($"ToQueryString: {kategorie.ToQueryString()}");

		foreach (Category k in kategorie)
		{
			WriteLine($"Kategoria {k.CategoryName} ma {k.Products.Count} produktów z przynajmniej {sztuki} sztukami w magazynie.");
			foreach (Product p in k.Products)
			{
				WriteLine($" Produkt {p.ProductName}: {p.WMagazynie} sztuk");
			}
		}
	}
}

static void ZapytanieOProdukty()
{
	using (Northwind db = new())
	{
		var fabrykaProtokolu = db.GetService<ILoggerFactory>();
		fabrykaProtokolu.AddProvider(new DostawcaProtokoluKonsoli());

		WriteLine("Produkty kosztujące więcej niż podana cena; posortowane:");
		string? wejscie;
		decimal cena;

		do
		{
			Write("Podaj cenę produktu: ");
			wejscie = ReadLine();
		} while (!decimal.TryParse(wejscie, out cena));

		IQueryable<Product>? produkty = db.Products?
			.TagWith("Produkty filtrowane według ceny i sortowane.") // Protokołowanie z wykorzystaniem znaczników zapytań
			.Where(produkt => produkt.Koszt > cena)
			.OrderByDescending(produkt => produkt.Koszt);

		if (produkty is null)
		{
			WriteLine("Nie znaleziono produktów.");
			return;
		}

		foreach (Product produkt in produkty)
		{
			WriteLine("{0}: {1} kosztuje {2:$#,##0.00}. W magazynie jest {3} sztuk.",
			  produkt.ProductID, produkt.ProductName, produkt.Koszt, produkt.WMagazynie);
		}
	}
}

static void ZapytanieZLike()
{
	using (Northwind db = new())
	{
		ILoggerFactory fabrykaProtokolu = db.GetService<ILoggerFactory>();
		fabrykaProtokolu.AddProvider(new DostawcaProtokoluKonsoli());

		Write("Wprowadź część nazwy produktu: ");
		string? input = ReadLine();

		IQueryable<Product>? produkty = db.Products?
		   .Where(p => EF.Functions.Like(p.ProductName, $"%{input}%"));

		if (produkty is null)
		{
			WriteLine("Nie znaleziono produktów.");
			return;
		}

		foreach (Product produkt in produkty)
		{
			WriteLine("{0}: w magazynie jest {1} sztuk. Produkt nie jest już wytwarzany? {2}",
			   produkt.ProductName, produkt.WMagazynie, produkt.Discontinued);
		}
	}
}

static bool DodajProdukt(int idKategorii, string nazwaProduktu, decimal? cena)
{
	using (Northwind db = new())
	{
		Product nowyProdukt = new()
		{
			CategoryID = idKategorii,
			ProductName = nazwaProduktu,
			Koszt = cena
		};

		// oznacz produkt jako dodany w systemie śledzenia zmian
		db.Products.Add(nowyProdukt);

		// zapisz wszystkie zmiany w bazie
		int zmienione = db.SaveChanges();
		return (zmienione == 1);
	}
}

static bool ZwiekszCeneProduktu(string poczatekNazwy, decimal kwota)
{
	using (var db = new Northwind())
	{
		// pobierz pierwszy produkt, którego nazwa zaczyna się od wartości parametru nazwa
		Product produktDoAktualizacji = db.Products.First(p => p.ProductName.StartsWith(poczatekNazwy));

		produktDoAktualizacji.Koszt += kwota;

		int zmienione = db.SaveChanges();
		return (zmienione == 1);
	}
}

static int UsunProdukty(string poczatekNazwy)
{
	using (Northwind db = new())
	{
		using (IDbContextTransaction t = db.Database.BeginTransaction())
		{
			WriteLine("Transakcja uruchomiona z poziomem izolacji: {0}",
			   arg0: t.GetDbTransaction().IsolationLevel);

			IQueryable<Product>? produkty = db.Products?.Where(p => p.ProductName.StartsWith(poczatekNazwy));

			if (produkty is null)
			{
				WriteLine("Nie znaleziono produktów do usunięcia.");
				return 0;
			}
			else
			{
				db.Products.RemoveRange(produkty);
			}

			int zmienione = db.SaveChanges();
			t.Commit();
			return zmienione;
		}
	}
}

static void WypiszProdukty()
{
	using (Northwind db = new())
	{
		WriteLine("{0,-3} {1,-35} {2,8} {3,5} {4}",
			"ID", "Nazwa", "Koszt", "Stan", "Nieprod.");

		foreach (Product pozycja in db.Products.OrderByDescending(p => p.Koszt))
		{
			WriteLine("{0:000} {1,-35} {2,8:$#,##0.00} {3,5} {4}",
			   pozycja.ProductID, pozycja.ProductName, pozycja.Koszt,
			   pozycja.WMagazynie, pozycja.Discontinued);
		}
	}
}