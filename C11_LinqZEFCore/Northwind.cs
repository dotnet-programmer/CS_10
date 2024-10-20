using Microsoft.EntityFrameworkCore;  // DbContext, DbSet<T>

namespace C10_WorkWithEFCore;

// klasa zarządzająca połączeniem z bazą danych
public class Northwind : DbContext
{
	// te właściwości opisują tabele w bazie danych
	public DbSet<Category>? Categories { get; set; }

	public DbSet<Product>? Products { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		string sciezka = Path.Combine(Environment.CurrentDirectory, "Northwind.db");

		optionsBuilder.UseSqlite($"Filename={sciezka}");

		/*
		string polaczenie = "Data Source=.;" +
		"Initial Catalog=Northwind;" +
		"Integrated Security=true;" +
		"MultipleActiveResultSets=true;";
		optionsBuilder.UseSqlServer(polaczenie);
		*/
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.Entity<Product>()
		  .Property(produkt => produkt.UnitPrice)
		  .HasConversion<double>();
}