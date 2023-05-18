namespace Library;

public class WyjatekOsoba : Exception
{
	public WyjatekOsoba() : base()
	{
	}

	public WyjatekOsoba(string komunikat) : base(komunikat)
	{
	}

	public WyjatekOsoba(string komunikat, Exception wewnetrznyWyjatek) : base(komunikat, wewnetrznyWyjatek)
	{
	}
}