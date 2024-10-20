using Microsoft.EntityFrameworkCore;
using static System.Console;

namespace UczniowieIKursy;

public class Academy : DbContext
{
	public DbSet<Student>? Students { get; set; }
	public DbSet<Course>? Courses { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		string path = Path.Combine(Environment.CurrentDirectory, "Akademia.db");

		WriteLine($"Używam pliku bazy danych {path}.");

		optionsBuilder.UseSqlite($"Filename={path}");

		// optionsBuilder.UseSqlServer(@"Data Source=.;InitialCatalog = Akademia; Integrated  // Security = true; MultipleActiveResultSets = true; ");
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// Reguły sprawdzania poprawności w płynnym API
		modelBuilder.Entity<Student>()
			.Property(s => s.Surname)
			.HasMaxLength(30)
			.IsRequired();

		// Wypełnienie bazy przykładowymi danymi
		Student stident1 = new()
		{
			StudentId = 1,
			Name = "Alicja",
			Surname = "Nowak"
		};
		Student student2 = new()
		{
			StudentId = 2,
			Name = "Bartek",
			Surname = "Kowalski"
		};
		Student student3 = new()
		{
			StudentId = 3,
			Name = "Celina",
			Surname = "Poranna"
		};

		Course course1 = new()
		{
			CourseId = 1,
			Name = "C# 10 i .NET 6",
		};
		Course course2 = new()
		{
			CourseId = 2,
			Name = "Tworzenie stron WWW",
		};
		Course course3 = new()
		{
			CourseId = 3,
			Name = "Python dla początkujących",
		};

		modelBuilder.Entity<Student>()
		  .HasData(stident1, student2, student3);

		modelBuilder.Entity<Course>()
		  .HasData(course1, course2, course3);

		modelBuilder.Entity<Course>()
		  .HasMany(c => c.Students)
		  .WithMany(u => u.Courses)
		  .UsingEntity(e => e.HasData(
			// Wszyscy uczniowie zapisani na kurs C#
			new { CoursesCourseId = 1, StudentsStudentId = 1 },
			new { CoursesCourseId = 1, StudentsStudentId = 2 },
			new { CoursesCourseId = 1, StudentsStudentId = 3 },
			// Tylko Bartek zapisał się na kurs tworzenia stron WWW
			new { CoursesCourseId = 2, StudentsStudentId = 2 },
			// Tylko Celina zapisała się na kurs Pythona
			new { CoursesCourseId = 3, StudentsStudentId = 3 }
	  ));
	}
}