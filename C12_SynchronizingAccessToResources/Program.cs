// Zaimportowanie przestrzeni nazw zawierającej typy diagnostyczne, takie jak Stopwatch.
// utworzenie klasy statycznej z dwoma polami:
// - pole generujące losowe czasy oczekiwania;
// - pole typu string przechowujące treść komunikatu (stanie się ono współdzielonym zasobem).
// zadeklarowanie dwóch metod, dodających literę A lub B do wspólnego ciągu znaków.
// Operację tę będą powtarzać w pętli pięć razy, przy każdej iteracji odczekując losowy czas, nie dłuższy niż 2 sekundy.

using System.Diagnostics;
using C12_SynchronizingAccessToResources;

Console.WriteLine("Proszę poczekać na zakończenie pracy przez zadania.");
Stopwatch stopwatch = Stopwatch.StartNew();

Task taskA = Task.Factory.StartNew(MethodA);
Task taskB = Task.Factory.StartNew(MethodB);

Task.WaitAll(taskA, taskB);

Console.WriteLine();
Console.WriteLine($"Wyniki: {CommonObjects.Message}.");
Console.WriteLine($"Upłynęło {stopwatch.ElapsedMilliseconds:#,##0} milisekund.");
Console.WriteLine($"Ciąg znaków zmieniono {CommonObjects.Licznik} razy.");

static void MethodA()
{
	// to rozwiązanie może powodować zakleszczenia
	//lock (CommonObjects.LockObject)
	//{
	//	for (int i = 0; i < 5; i++)
	//	{
	//		Thread.Sleep(CommonObjects.Random.Next(2000));
	//		CommonObjects.Message += "A";
	//		Console.Write(".");
	//	}
	//}

	// to rozwiązanie pozwala uniknąć zakleszczeń
	try
	{
		if (Monitor.TryEnter(CommonObjects.LockObject, TimeSpan.FromSeconds(15)))
		{
			for (int i = 0; i < 5; i++)
			{
				Thread.Sleep(CommonObjects.Random.Next(2000));
				CommonObjects.Message += "A";
				// tworzenie niepodzielnych operacji
				Interlocked.Increment(ref CommonObjects.Licznik);
				Console.Write(".");
			}
		}
		else
		{
			Console.WriteLine("Przekroczenie czasu prób wejścia do monitora konchy.");
		}
	}
	finally
	{
		Monitor.Exit(CommonObjects.LockObject);
	}
}

static void MethodB()
{
	//lock (CommonObjects.LockObject)
	//{
	//	for (int i = 0; i < 5; i++)
	//	{
	//		Thread.Sleep(CommonObjects.Random.Next(2000));
	//		CommonObjects.Message += "B";
	//		Console.Write(".");
	//	}
	//}

	try
	{
		if (Monitor.TryEnter(CommonObjects.LockObject, TimeSpan.FromSeconds(15)))
		{
			for (int i = 0; i < 5; i++)
			{
				Thread.Sleep(CommonObjects.Random.Next(2000));
				CommonObjects.Message += "B";
				// tworzenie niepodzielnych operacji
				Interlocked.Increment(ref CommonObjects.Licznik);
				Console.Write(".");
			}
		}
		else
		{
			Console.WriteLine("Przekroczenie czasu prób wejścia do monitora konchy.");
		}
	}
	finally
	{
		Monitor.Exit(CommonObjects.LockObject);
	}
}