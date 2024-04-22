namespace Library;

public class Person : object, IComparable<Person>
{
	// pola
	public string? Nazwisko; // znak ? pozwala na wartość null

	public DateTime DataUrodzenia;
	public List<Person> Dzieci = new(); // język C# 9 i nowsze

	// metody
	public void WypiszWKonsoli() 
		=> Console.WriteLine($"{Nazwisko}, data urodzenia {DataUrodzenia:dddd}.");

	// statyczna metoda "rozmnażania"
	public static Person Prokreacja(Person o1, Person o2)
	{
		Person dziecko = new()
		{
			Nazwisko = $"Dziecko osób {o1.Nazwisko} i {o2.Nazwisko}"
		};

		o1.Dzieci.Add(dziecko);
		o2.Dzieci.Add(dziecko);

		return dziecko;
	}

	// "rozmnażająca" metoda obiektu
	public Person ProkreacjaZ(Person partner) 
		=> Prokreacja(this, partner);

	// operator "mnożenia"
	public static Person operator *(Person o1, Person o2) 
		=> Prokreacja(o1, o2);

	// metoda z funkcją lokalną
	public static int Silnia(int liczba)
	{
		return liczba < 0 ? throw new ArgumentException($"{nameof(liczba)} nie może być mniejsza od zera.") : lokalnaSilnia(liczba);

		// funkcja lokalna
		static int lokalnaSilnia(int lokalnaLiczba) 
			=> lokalnaLiczba < 1 ? 1 : lokalnaLiczba * lokalnaSilnia(lokalnaLiczba - 1);
	}

	// zdarzenie
	public event EventHandler? Krzycz;

	// pole
	public int PoziomZlosci;

	// metoda
	public void Szturchnij()
	{
		PoziomZlosci++;
		if (PoziomZlosci >= 3)
		{
			//// jeżeli coś słucha zdarzenia…
			//if (Krzycz != null)
			//{
			//	// …to wywołaj zdarzenie
			//	Krzycz(this, EventArgs.Empty);
			//}

			// to samo co wyżej, ale prostszy zapis
			Krzycz?.Invoke(this, EventArgs.Empty);
		}
	}

	public int CompareTo(Person? other) 
		=> Nazwisko is null ? 0 : Nazwisko.CompareTo(other?.Nazwisko);

	public override string ToString() 
		=> $"{Nazwisko} to {base.ToString()}";

	public void PodrozWCzasie(DateTime kiedy)
	{
		if (kiedy <= DataUrodzenia)
		{
			throw new WyjatekOsoba("Jeżeli przeniesiesz się w czasie do daty wcześniejszej niż Twoja data urodzenia, to cały wszechświat eksploduje!");
		}
		else
		{
			Console.WriteLine($"Witam w roku {kiedy:yyyy}!");
		}
	}
}