using Microsoft.EntityFrameworkCore; // UseSqlServer
using Microsoft.Extensions.DependencyInjection; // IServiceCollection

namespace CommonLibrary;

public static class NorthwindContextExtensions
{
	/// <summary>
	/// Dodaje obiekt typu NorthwindContext do wskazanej kolekcji
	/// IServiceCollection. Używa dostawcy danych SqlServer.
	/// </summary>
	/// <param name="services"></param>
	/// <param name="connection">Przypisz wartość, aby zmienić domyślną.</param>
	/// <returns>Kolekcja typu IServiceCollection, której można użyć do dodawania
	/// kolejnych serwisów.</returns>
	public static IServiceCollection AddNorthwindContext(
		this IServiceCollection services,
		string connection = "Server=(local)\\SQLEXPRESS;Database=Northwind;User Id=DBUser;Password=1234;TrustServerCertificate=True;")
	{
		services.AddDbContext<NorthwindContext>(options => options.UseSqlServer(connection));
		return services;
	}
}