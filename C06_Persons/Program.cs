using Library;

var henryk = new Person { Nazwisko = "Henryk" };
var maria = new Person { Nazwisko = "Maria" };
var julia = new Person { Nazwisko = "Julia" };

// wywołanie metody obiektu
var dziecko1 = maria.ProkreacjaZ(henryk);
//dziecko1.Nazwisko = "Grzegorz";

// wywołanie metody statycznej
var dziecko2 = Person.Prokreacja(henryk, julia);

// wywołanie operatora
var dziecko3 = henryk * maria;

Console.WriteLine($"{maria.Nazwisko} ma {maria.Dzieci.Count} dzieci.");
Console.WriteLine($"{henryk.Nazwisko} ma {henryk.Dzieci.Count} dzieci.");
Console.WriteLine($"{julia.Nazwisko} ma {julia.Dzieci.Count} dzieci.");
Console.WriteLine(
   format: "Pierwsze dziecko {0} nazywa się \"{1}\".",
   arg0: maria.Nazwisko,
   arg1: maria.Dzieci[0].Nazwisko);

Console.WriteLine($"5! to {Person.Silnia(5)}");

henryk.Krzycz += Henryk_Krzycz;

static void Henryk_Krzycz(object? sender, EventArgs e)
{
	if (sender is null)
	{
		return;
	}

	Person o = (Person)sender;
	Console.WriteLine($"{o.Nazwisko} złości się na poziomie: {o.PoziomZlosci}.");
}

henryk.Szturchnij();
henryk.Szturchnij();
henryk.Szturchnij();
henryk.Szturchnij();
henryk.Szturchnij();

Person[] osoby =
{
   new() { Nazwisko = "Saniak" },
   new() { Nazwisko = "Janiak" },
   new() { Nazwisko = "Adun" },
   new() { Nazwisko = "Rykszak" }
};

Console.WriteLine("Początkowa lista osób:");
foreach (var Person in osoby)
{
	Console.WriteLine($"  {Person.Nazwisko}");
}

Console.WriteLine("Do posortowania użyto zaimplementowanego interfejsu IComparable:");
Array.Sort(osoby);
foreach (var Person in osoby)
{
	Console.WriteLine($"  {Person.Nazwisko}");
}

Console.WriteLine("Do posortowania użyto implementacji interfejsu IComparer w klasie OsobaComparer:");
Array.Sort(osoby, new PersonComparer());
foreach (var osoba in osoby)
{
	Console.WriteLine($"{osoba.Nazwisko}");
}

WektorPrzesuniecia wp1 = new(3, 5);
WektorPrzesuniecia wp2 = new(-2, 7);
WektorPrzesuniecia wp3 = wp1 + wp2;
Console.WriteLine($"({wp1.X}, {wp1.Y}) + ({wp2.X}, {wp2.Y}) = ({wp3.X}, {wp3.Y})");

Pracownik p1 = new()
{
	Nazwisko = "Jacek Jankowski",
	DataUrodzenia = new DateTime(1990, 7, 28)
};
p1.WypiszWKonsoli();
p1.KodPracownika = "JJ001";
p1.DataZatrudnienia = new DateTime(2014, 11, 23);
Console.WriteLine($"{p1.Nazwisko} {p1.KodPracownika} został zatrudniony {p1.DataZatrudnienia:dd/MM/yy}");
Console.WriteLine(p1.ToString());

Pracownik pracownikAlicja = new() { Nazwisko = "Alicja", KodPracownika = "AA123" };
Person osobaAlicja = pracownikAlicja;
pracownikAlicja.WypiszWKonsoli();
osobaAlicja.WypiszWKonsoli();
Console.WriteLine(pracownikAlicja.ToString());
Console.WriteLine(osobaAlicja.ToString());

//Pracownik p2 = (Pracownik)osobaAlicja;

//if (osobaAlicja is Pracownik)
//{
//	Console.WriteLine($"{nameof(osobaAlicja)} jest Pracownikiem");
//	Pracownik p2 = (Pracownik)osobaAlicja;
//	// zrób coś ze zmienną p2
//}

if (osobaAlicja is Pracownik p2)
{
	Console.WriteLine($"{nameof(osobaAlicja)} jest Pracownikiem");
	// zrób coś ze zmienną p2
}

Pracownik? p3 = osobaAlicja as Pracownik; // może zwrócić null
if (p3 != null)
{
	Console.WriteLine($"{nameof(osobaAlicja)} jako Pracownik");
	// zrób coś ze zmienną p3
}

try
{
	p1.PodrozWCzasie(new DateTime(1999, 12, 31));
	p1.PodrozWCzasie(new DateTime(1950, 12, 25));
}
catch (WyjatekOsoba ex)
{
	Console.WriteLine(ex.Message);
}

string email1 = "pamela@test.com";
string email2 = "jan&test.com";
Console.WriteLine(
   "{0} to poprawny adres e-mail: {1}.",
   arg0: email1,
   arg1: RozszerzeniaDlaString.AdresPoprawny(email1));

Console.WriteLine(
   "{0} to poprawny adres e-mail: {1}.",
	arg0: email2,
	arg1: RozszerzeniaDlaString.AdresPoprawny(email2));

Console.WriteLine(
   "{0} to poprawny adres e-mail:{1}.",
   arg0: email1,
   arg1: email1.AdresPoprawny());

Console.WriteLine(
   "{0} to poprawny adres e-mail:{1}.",
   arg0: email2,
   arg1: email2.AdresPoprawny());

Console.Write("Podaj poprawny adres w internecie: ");
String? url = Console.ReadLine();

if (string.IsNullOrWhiteSpace(url))
{
	url = "https://stackoverflow.com/search?q=securestring";
}

Uri uri = new(url);

Console.WriteLine($"URL: {url}");
Console.WriteLine($"Protokół: {uri.Scheme}");
Console.WriteLine($"Port: {uri.Port}");
Console.WriteLine($"Host: {uri.Host}");
Console.WriteLine($"Ścieżka: {uri.AbsolutePath}");
Console.WriteLine($"Zapytanie: {uri.Query}");