namespace System.Linq;  // rozszerzamy przestrzeń nazw Microsoftu

public static class MyLinqExtensions
{
	// metoda rozszerzająca LINQ
	public static IEnumerable<T> ProcessSequences<T>(this IEnumerable<T> sequence) 
		=> sequence; // tu można przetwarzać sekwencję

	public static IQueryable<T> ProcessSequences<T>(this IQueryable<T> sequence) 
		=> sequence; // tu można przetwarzać sekwencję

	// skalarna metoda rozszerzająca LINQ
	public static int? Median(this IEnumerable<int?> sequence)
	{
		var sorted = sequence.OrderBy(element => element);
		int middle = sorted.Count() / 2;
		return sorted.ElementAt(middle);
	}

	public static int? Median<T>(this IEnumerable<T> sequence, Func<T, int?> selector) 
		=> sequence.Select(selector).Median();

	public static decimal? Median(this IEnumerable<decimal?> sequence)
	{
		var sorted = sequence.OrderBy(element => element);
		int middle = sorted.Count() / 2;
		return sorted.ElementAt(middle);
	}

	public static decimal? Median<T>(this IEnumerable<T> sequence, Func<T, decimal?> selector) 
		=> sequence.Select(selector).Median();

	public static int? Dominant(this IEnumerable<int?> sequence)
	{
		var grouped = sequence.GroupBy(element => element);
		var sortedGroups = grouped.OrderBy(group => group.Count());
		return sortedGroups.FirstOrDefault()?.Key;
	}

	public static int? Dominant<T>(this IEnumerable<T> sequence, Func<T, int?> selector) 
		=> sequence.Select(selector)?.Dominant();

	public static decimal? Dominant(this IEnumerable<decimal?> sequence)
	{
		var grouped = sequence.GroupBy(element => element);
		var sortedGroups = grouped.OrderBy(group => group.Count());
		return sortedGroups.FirstOrDefault()?.Key;
	}

	public static decimal? Dominant<T>(this IEnumerable<T> sequence, Func<T, decimal?> selector) 
		=> sequence.Select(selector).Dominant();
}