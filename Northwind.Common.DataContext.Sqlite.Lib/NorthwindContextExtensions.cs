using Microsoft.EntityFrameworkCore; // UseSqlite
using Microsoft.Extensions.DependencyInjection; // IServiceCollection

namespace CommonLibrary;

public static class NorthwindContextExtensions
{
	/// <summary>
	/// Dodaje obiekt NorthwindContext do wskazanej kolekcji typu IServiceCollection. Używa dostawcy bazy danych Sqlite.
	/// </summary>
	/// <param name="services"></param>
	/// <param name="relativePath">Ta wartość podmienia domyślną ścieżkę ".."</param>
	/// <returns>Kolekcja typu IServiceCollection, której można użyć do dodawania kolejnych serwisów.</returns>
	public static IServiceCollection AddNorthwindContextSqlite(this IServiceCollection services, string relativePath = "..")
	{
		string dataBasePath = Path.Combine(relativePath, "Northwind.db");
		services.AddDbContext<NorthwindContext>(o => o.UseSqlite($"Data Source={relativePath}"));
		return services;
	}
}