using Microsoft.EntityFrameworkCore; // GenerateCreateScript()
using UczniowieIKursy; // Akademia
using static System.Console;

//W pliku Program.cs dopisz instrukcje tworzące obiekt kontekstu danych klasy Akademia,
//używające go do usunięcia ewentualnie istniejącej bazy danych i utworzenia nowej na podstawie modelu
//i wypisujące wykorzystany skrypt SQL, a następnie wypisujące listy uczniów i kursów:
using (Academy academy = new())
{
	bool isDeleted = await academy.Database.EnsureDeletedAsync();
	WriteLine($"Usunięto bazę danych: {isDeleted}");

	bool isCreated = await academy.Database.EnsureCreatedAsync();
	WriteLine($"Utworzono bazę danych: {isCreated}");

	WriteLine("Skrypt SQL użyty do utworzenia bazy danych:");
	WriteLine(academy.Database.GenerateCreateScript());

	foreach (Student student in academy.Students.Include(s => s.Courses))
	{
		WriteLine("{0} {1} uczęszcza na {2} kursy:",
		  student.Name,
		  student.Surname,
		  student.Courses.Count);

		foreach (Course course in student.Courses)
		{
			WriteLine($" {course.Name}");
		}
	}
}