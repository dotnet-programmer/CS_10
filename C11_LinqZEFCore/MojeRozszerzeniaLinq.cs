namespace System.Linq;  // rozszerzamy przestrzeń nazw Microsoftu

public static class MojeRozszerzeniaLINQ
{
	// metoda rozszerzająca LINQ
	public static IEnumerable<T> PrzetwarzajSekwencje<T>(
	  this IEnumerable<T> sekwencja) =>
		// tu można przetwarzać sekwencję
		sekwencja;

	public static IQueryable<T> PrzetwarzajSekwencje<T>(
	  this IQueryable<T> sekwencja) =>
		// tu można przetwarzać sekwencję
		sekwencja;

	// skalarna metoda rozszerzająca LINQ
	public static int? Mediana(
	  this IEnumerable<int?> sekwencja)
	{
		var uporzadkowana = sekwencja.OrderBy(element => element);
		int pozycjaSrodkowa = uporzadkowana.Count() / 2;
		return uporzadkowana.ElementAt(pozycjaSrodkowa);
	}

	public static int? Mediana<T>(
	  this IEnumerable<T> sekwencja, Func<T, int?> selektor) => sekwencja.Select(selektor).Mediana();

	public static decimal? Mediana(
	  this IEnumerable<decimal?> sekwencja)
	{
		var uporzadkowana = sekwencja.OrderBy(element => element);
		int pozycjaSrodkowa = uporzadkowana.Count() / 2;
		return uporzadkowana.ElementAt(pozycjaSrodkowa);
	}

	public static decimal? Mediana<T>(
	  this IEnumerable<T> sekwencja, Func<T, decimal?> selektor) => sekwencja.Select(selektor).Mediana();

	public static int? Dominanta(this IEnumerable<int?> sekwencja)
	{
		var zgrupowane = sekwencja.GroupBy(element => element);
		var uporzadkowaneGrupy = zgrupowane.OrderBy(
		  grupa => grupa.Count());
		return uporzadkowaneGrupy.FirstOrDefault()?.Key;
	}

	public static int? Dominanta<T>(
	  this IEnumerable<T> sekwencja, Func<T, int?> selektor) => sekwencja.Select(selektor)?.Dominanta();

	public static decimal? Dominanta(
	  this IEnumerable<decimal?> sekwencja)
	{
		var zgrupowane = sekwencja.GroupBy(element => element);
		var uporzadkowaneGrupy = zgrupowane.OrderBy(
		  grupa => grupa.Count());
		return uporzadkowaneGrupy.FirstOrDefault()?.Key;
	}

	public static decimal? Dominanta<T>(
	  this IEnumerable<T> sekwencja, Func<T, decimal?> selektor) => sekwencja.Select(selektor).Dominanta();
}