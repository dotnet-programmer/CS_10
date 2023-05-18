using System.Diagnostics;
using static System.Console;

Stopwatch zegar = new();
Write("Naciśnij klawisz ENTER. ");
ReadLine();
zegar.Start();

int max = 45;

IEnumerable<int> liczby = Enumerable.Range(start: 1, count: max);

WriteLine($"Wyliczam sekwencję pierwszych {max} liczb ciągu Fibonacciego. Proszę czekać...");

//int[] liczbyFibonacciego = liczby.Select(liczba => Fibonacci(liczba)).ToArray();

int[] liczbyFibonacciego = liczby.AsParallel()
  .Select(liczba => Fibonacci(liczba))
  .OrderBy(liczba => liczba)
  .ToArray();

zegar.Stop();
WriteLine("Upłynęło {0:#,##0} milisekund.",
  arg0: zegar.ElapsedMilliseconds);

Write("Wyniki:");
foreach (int liczba in liczbyFibonacciego)
{
	Write($" {liczba}");
}

static int Fibonacci(int liczba) =>
  liczba switch
  {
	  1 => 0,
	  2 => 1,
	  _ => Fibonacci(liczba - 1) + Fibonacci(liczba - 2)
  };