namespace Library;

public class PersonComparer : IComparer<Person>
{
	public int Compare(Person? x, Person? y)
	{
		if (x == null || y == null)
		{
			return 0;
		}

		// porównaj długości nazwisk...
		int wynik = x.Nazwisko.Length.CompareTo(y.Nazwisko.Length);

		// …a jeżeli są równe…
		if (wynik == 0)
		{
			// …to posortuj alfabetycznie…
			return x.Nazwisko.CompareTo(y.Nazwisko);
		}
		else
		{
			// …poza tym sortuj według długości
			return wynik;
		}
	}
}