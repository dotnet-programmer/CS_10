using Microsoft.EntityFrameworkCore; // DbContext, DbContextOptionsBuilder
using static System.Console;

namespace C10_WorkWithEFCore;

// zajmuje się obsługą połączenia z bazą danych
public class Northwind : DbContext
{
	// te właściwości odwzorowują tabele z bazy danych
	public DbSet<Category> Categories { get; set; }
	public DbSet<Product> Products { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		// leniwe ładowanie danych z DB
		optionsBuilder.UseLazyLoadingProxies();

		if (ProjectConstants.DataProvider == "SQLite")
		{
			string path = Path.Combine(Environment.CurrentDirectory, "Northwind.db");
			WriteLine($"Używam pliku bazy danych {path}.");
			optionsBuilder.UseSqlite($"Filename={path}");
		}
		else
		{
			string connection = "Data Source=.;" +
			  "Initial Catalog=Northwind;" +
			  "Integrated Security=true;" +
			  "MultipleActiveResultSets=true;";

			optionsBuilder.UseSqlServer(connection);
		}
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// przykład użycia płynnego API zamiast atrybutów, aby ograniczyć długość nazwy kategorii do 40 znaków
		modelBuilder.Entity<Category>()
		   .Property(category => category.CategoryName)
		   .IsRequired()  // NOT NULL
		   .HasMaxLength(40);

		if (ProjectConstants.DataProvider == "SQLite")
		{
			// dodane w celu "naprawienia" braku typu decimal w SQLite
			modelBuilder.Entity<Product>()
			   .Property(product => product.Price).HasConversion<double>();
		}

		// zasiewanie danych
		modelBuilder.Entity<Product>()
			.HasData(new Product
			{
				ProductID = 1,
				ProductName = "ProductName",
				Price = 8.99M
			});

		// globalny filtr usuwający produkty nieprodukowane
		modelBuilder.Entity<Product>()
			.HasQueryFilter(p => !p.Discontinued);
	}

	// Własne konfigurowanie konwencji
	protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
	{
		configurationBuilder.Properties<string>().HaveMaxLength(50);
		//configurationBuilder.IgnoreAny<INieOdwzorowuj>();
	}
}