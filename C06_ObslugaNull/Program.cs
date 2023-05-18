int nieMozeBycNull = 4;
// nieMozeBycNull = null; // błąd kompilacji!

int? mozeBycNull = null;
Console.WriteLine(mozeBycNull);
Console.WriteLine(mozeBycNull.GetValueOrDefault());

mozeBycNull = 7;
Console.WriteLine(mozeBycNull);
Console.WriteLine(mozeBycNull.GetValueOrDefault());

Adres adres = new()
{
	Budynek = null,
	Ulica = null,
	Miasto = "Wrocław",
	Region = null
};

internal class Adres
{
	public string? Budynek;
	public string Ulica = string.Empty;
	public string Miasto = string.Empty;
	public string Region = string.Empty;
}