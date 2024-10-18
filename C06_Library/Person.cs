namespace Library;

public class Person : object, IComparable<Person>
{
	// pola
	public string? Name; // znak ? pozwala na wartość null

	public DateTime DateOfBirth;
	public List<Person> Kids = []; // język C# 9 i nowsze

	// metody
	public void WriteLineInConsole()
		=> Console.WriteLine($"{Name}, data urodzenia {DateOfBirth:dddd}.");

	// statyczna metoda "rozmnażania"
	public static Person Procreation(Person o1, Person o2)
	{
		ArgumentNullException.ThrowIfNull(o1);
		ArgumentNullException.ThrowIfNull(o2);

		Person child = new()
		{
			Name = $"Dziecko osób {o1.Name} i {o2.Name}"
		};

		o1.Kids.Add(child);
		o2.Kids.Add(child);

		return child;
	}

	// "rozmnażająca" metoda obiektu
	public Person ProcreationWith(Person partner)
		=> Procreation(this, partner);

	// operator "mnożenia"
	public static Person operator *(Person o1, Person o2)
		=> Procreation(o1, o2);

	// metoda z funkcją lokalną
	public static int Factorial(int number)
	{
		return number < 0
			? throw new ArgumentException($"{nameof(number)} nie może być mniejsza od zera.")
			: FactorialLocalMethod(number);

		// funkcja lokalna
		static int FactorialLocalMethod(int LocalNumber)
			=> LocalNumber < 1
				? 1
				: LocalNumber * FactorialLocalMethod(LocalNumber - 1);
	}

	// zdarzenie
	public event EventHandler? ShoutOut;

	// pole
	public int AngerLevel;

	// metoda
	public void Hit()
	{
		AngerLevel++;
		if (AngerLevel >= 3)
		{
			//// jeżeli coś słucha zdarzenia…
			//if (ShoutOut != null)
			//{
			//	// …to wywołaj zdarzenie
			//	ShoutOut(this, EventArgs.Empty);
			//}

			// to samo co wyżej, ale prostszy zapis
			ShoutOut?.Invoke(this, EventArgs.Empty);
		}
	}

	public int CompareTo(Person? other)
		=> Name is null ? 0 : Name.CompareTo(other?.Name);

	public override string ToString()
		=> $"{Name} to {base.ToString()}";

	public void TimeTravel(DateTime date)
	{
		if (date <= DateOfBirth)
		{
			throw new PersonException("Jeżeli przeniesiesz się w czasie do daty wcześniejszej niż Twoja data urodzenia, to cały wszechświat eksploduje!");
		}
		Console.WriteLine($"Witam w roku {date:yyyy}!");
	}
}