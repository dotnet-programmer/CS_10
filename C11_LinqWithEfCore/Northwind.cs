using Microsoft.EntityFrameworkCore;  // DbContext, DbSet<T>

namespace C11_LinqWithEfCore;

// klasa zarządzająca połączeniem z bazą danych
public class Northwind : DbContext
{
	// te właściwości opisują tabele w bazie danych
	public DbSet<Category>? Categories { get; set; }
	public DbSet<Product>? Products { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		string path = Path.Combine(Environment.CurrentDirectory, "Northwind.db");
		optionsBuilder.UseSqlite($"Filename={path}");

		/*
		string polaczenie = "Data Source=.;" +
		"Initial Catalog=Northwind;" +
		"Integrated Security=true;" +
		"MultipleActiveResultSets=true;";
		optionsBuilder.UseSqlServer(polaczenie);
		*/
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder) 
		=> modelBuilder.Entity<Product>()
		  .Property(p => p.UnitPrice)
		  .HasConversion<double>();
}