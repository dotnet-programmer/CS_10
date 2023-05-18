using Microsoft.EntityFrameworkCore; // GenerateCreateScript()
using UczniowieIKursy; // Akademia

using static System.Console;

//W pliku Program.cs dopisz instrukcje tworzące obiekt kontekstu danych klasy Akademia,
//używające go do usunięcia ewentualnie istniejącej bazy danych i utworzenia nowej na podstawie modelu
//i wypisujące wykorzystany skrypt SQL, a następnie wypisujące listy uczniów i kursów:
using (Akademia a = new())
{
	bool usuniete = await a.Database.EnsureDeletedAsync();
	WriteLine($"Usunięto bazę danych: {usuniete}");

	bool utworzone = await a.Database.EnsureCreatedAsync();
	WriteLine($"Utworzono bazę danych: {utworzone}");

	WriteLine("Skrypt SQL użyty do utworzenia bazy danych:");
	WriteLine(a.Database.GenerateCreateScript());

	foreach (Uczen u in a.Uczniowie.Include(s => s.Kursy))
	{
		WriteLine("{0} {1} uczęszcza na {2} kursy:",
		  u.Imie,
		  u.Nazwisko,
		  u.Kursy.Count);

		foreach (Kurs k in u.Kursy)
		{
			WriteLine($" {k.Nazwa}");
		}
	}
}