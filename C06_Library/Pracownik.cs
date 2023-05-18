namespace Library;

public class Pracownik : Person
{
	public string? KodPracownika { get; set; }
	public DateTime DataZatrudnienia { get; set; }

	public void WypiszWKonsoli() => Console.WriteLine(format:
		   "{0}, data urodzenia {1:dd/MM/yy}, data zatrudnienia {2:dd/MM/yy}",
		   arg0: Nazwisko,
		   arg1: DataUrodzenia,
		   arg2: DataZatrudnienia);

	public override string ToString() => $"Kod pracownika {Nazwisko} to {KodPracownika}";
}