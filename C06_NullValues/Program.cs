int cannotBeNull = 4;
// cannotBeNull = null; // błąd kompilacji!

int? mayBeNull = null;
Console.WriteLine(mayBeNull);
Console.WriteLine(mayBeNull.GetValueOrDefault());

mayBeNull = 7;
Console.WriteLine(mayBeNull);
Console.WriteLine(mayBeNull.GetValueOrDefault());

Address address = new()
{
	Building = null,
	Street = null,
	City = "Wrocław",
	Region = null
};

internal class Address
{
	public string? Building;
	public string Street = string.Empty;
	public string City = string.Empty;
	public string Region = string.Empty;
}