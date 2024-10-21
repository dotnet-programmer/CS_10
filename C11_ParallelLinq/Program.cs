using System.Diagnostics;
using static System.Console;

Write("Naciśnij klawisz ENTER. ");
ReadLine();
int max = 45;
IEnumerable<int> numbers = Enumerable.Range(start: 1, count: max);
WriteLine($"Wyliczam sekwencję pierwszych {max} liczb ciągu Fibonacciego. Proszę czekać...");
WriteLine("\nLINQ:");
FibonacciNotParallel(numbers);
WriteLine("\nParalell LINQ:");
FibonacciParallel(numbers);

static void FibonacciNotParallel(IEnumerable<int> numbers)
{
	Stopwatch stopwatch = Stopwatch.StartNew();
	int[] FibonacciNumbers = numbers.Select(Fibonacci).ToArray();
	stopwatch.Stop();
	WriteResults(stopwatch, FibonacciNumbers);
}

static void FibonacciParallel(IEnumerable<int> numbers)
{
	Stopwatch stopwatch = Stopwatch.StartNew();
	int[] FibonacciNumbers = numbers.AsParallel()
	  .Select(Fibonacci)
	  .OrderBy(x => x)
	  .ToArray();
	stopwatch.Stop();
	WriteResults(stopwatch, FibonacciNumbers);
}

static int Fibonacci(int number) =>
  number switch
  {
	  1 => 0,
	  2 => 1,
	  _ => Fibonacci(number - 1) + Fibonacci(number - 2)
  };

static void WriteResults(Stopwatch stopwatch, int[] FibonacciNumbers)
{
	WriteLine($"Upłynęło {stopwatch.ElapsedMilliseconds:#,##0} milisekund. Wyniki:");
	foreach (var item in FibonacciNumbers)
	{
		Write($"{item} ");
	}
	WriteLine();
}