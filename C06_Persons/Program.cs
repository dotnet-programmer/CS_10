using Library;

var henryk = new Person { Name = "Henryk" };
var maria = new Person { Name = "Maria" };
var julia = new Person { Name = "Julia" };

// wywołanie metody obiektu
var child1 = maria.ProcreationWith(henryk);

// wywołanie metody statycznej
var child2 = Person.Procreation(henryk, julia);

// wywołanie operatora
var child3 = henryk * maria;

Console.WriteLine($"{maria.Name} ma {maria.Kids.Count} dzieci.");
Console.WriteLine($"{henryk.Name} ma {henryk.Kids.Count} dzieci.");
Console.WriteLine($"{julia.Name} ma {julia.Kids.Count} dzieci.");
Console.WriteLine(
   format: "Pierwsze dziecko {0} nazywa się \"{1}\".",
   arg0: maria.Name,
   arg1: maria.Kids[0].Name);

Console.WriteLine($"5! to {Person.Factorial(5)}");

henryk.ShoutOut += Henryk_ShoutOut;

static void Henryk_ShoutOut(object? sender, EventArgs e)
{
	if (sender is null)
	{
		return;
	}

	Person person = (Person)sender;
	Console.WriteLine($"{person.Name} złości się na poziomie: {person.AngerLevel}.");
}

henryk.Hit();
henryk.Hit();
henryk.Hit();
henryk.Hit();
henryk.Hit();

Person[] persons =
{
   new() { Name = "Saniak" },
   new() { Name = "Janiak" },
   new() { Name = "Adun" },
   new() { Name = "Rykszak" }
};

Console.WriteLine("Początkowa lista osób:");
foreach (var Person in persons)
{
	Console.WriteLine($"  {Person.Name}");
}

Console.WriteLine("Do posortowania użyto zaimplementowanego interfejsu IComparable:");
Array.Sort(persons);
foreach (var Person in persons)
{
	Console.WriteLine($"  {Person.Name}");
}

Console.WriteLine("Do posortowania użyto implementacji interfejsu IComparer w klasie OsobaComparer:");
Array.Sort(persons, new PersonComparer());
foreach (var item in persons)
{
	Console.WriteLine($"{item.Name}");
}

DisplacementVector wp1 = new(3, 5);
DisplacementVector wp2 = new(-2, 7);
DisplacementVector wp3 = wp1 + wp2;
Console.WriteLine($"({wp1.X}, {wp1.Y}) + ({wp2.X}, {wp2.Y}) = ({wp3.X}, {wp3.Y})");

Employee employee = new()
{
	Name = "Jacek Jankowski",
	DateOfBirth = new DateTime(1990, 7, 28)
};
employee.WriteLineInConsole();
employee.EmployeeCode = "JJ001";
employee.DateOfEmployment = new DateTime(2014, 11, 23);
Console.WriteLine($"{employee.Name} {employee.EmployeeCode} został zatrudniony {employee.DateOfEmployment:dd/MM/yy}");
Console.WriteLine(employee.ToString());

Employee employeeAlicja = new() { Name = "Alicja", EmployeeCode = "AA123" };
Person personAlicja = employeeAlicja;
employeeAlicja.WriteLineInConsole();
personAlicja.WriteLineInConsole();
Console.WriteLine(employeeAlicja.ToString());
Console.WriteLine(personAlicja.ToString());

if (personAlicja is Employee p2)
{
	Console.WriteLine($"{nameof(personAlicja)} jest Pracownikiem");
	// zrób coś ze zmienną p2
}

Employee? p3 = personAlicja as Employee; // może zwrócić null
if (p3 != null)
{
	Console.WriteLine($"{nameof(personAlicja)} jako Pracownik");
	// zrób coś ze zmienną p3
}

try
{
	employee.TimeTravel(new DateTime(1999, 12, 31));
	employee.TimeTravel(new DateTime(1950, 12, 25));
}
catch (PersonException ex)
{
	Console.WriteLine(ex.Message);
}

string email1 = "pamela@test.com";
string email2 = "jan&test.com";
Console.WriteLine(
   "{0} to poprawny adres e-mail: {1}.",
   arg0: email1,
   arg1: StringExtension.CorrectEmail(email1));

Console.WriteLine(
   "{0} to poprawny adres e-mail: {1}.",
	arg0: email2,
	arg1: StringExtension.CorrectEmail(email2));

Console.WriteLine(
   "{0} to poprawny adres e-mail:{1}.",
   arg0: email1,
   arg1: email1.CorrectEmail());

Console.WriteLine(
   "{0} to poprawny adres e-mail:{1}.",
   arg0: email2,
   arg1: email2.CorrectEmail());

Console.Write("Podaj poprawny adres w internecie: ");
string? url = Console.ReadLine();

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