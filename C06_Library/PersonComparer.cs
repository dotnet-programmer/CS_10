namespace Library;

public class PersonComparer : IComparer<Person>
{
	public int Compare(Person? person1, Person? person2)
	{
		if (person1 == null || person2 == null)
		{
			return 0;
		}

		// porównaj długości nazwisk...
		int result = person1.Name!.Length.CompareTo(person2.Name!.Length);

		return result switch
		{
			// …a jeżeli są równe to posortuj alfabetycznie…
			0 => person1.Name.CompareTo(person2.Name),
			// …poza tym sortuj według długości
			_ => result
		};
	}
}