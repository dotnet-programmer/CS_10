using System.Diagnostics; // Stopwatch
using static System.Console;

TestMethod1();
WriteLine();
ReadLine();

TestMethod2();
WriteLine();
ReadLine();

TestMethod3();
WriteLine();
ReadLine();

static void TestMethod1()
{
	WriteThreadDetails();
	WriteLine("\nUruchamianie metod synchronicznie, w jednym wątku.\n");
	Stopwatch stopwatch = Stopwatch.StartNew();
	MethodSimulatingWorkA();
	WriteLine();
	MethodSimulatingWorkB();
	WriteLine();
	MethodSimulatingWorkC();
	stopwatch.Stop();
	WriteLine($"\nUpłynęło {stopwatch.ElapsedMilliseconds:#,##0}ms.");
}

static void TestMethod2()
{
	WriteThreadDetails();
	WriteLine("\nUruchamianie metod asynchroniczne, w wielu wątkach.\n");
	Stopwatch stopwatch = Stopwatch.StartNew();

	// 3 różne sposoby uruchamiania metod za pomocą obiektów klasy Task

	Task taskA = new(MethodSimulatingWorkA);
	taskA.Start();

	Task taskB = Task.Factory.StartNew(MethodSimulatingWorkB);

	Task taskC = Task.Run(MethodSimulatingWorkC);

	// Czasami przed podjęciem dalszych działań trzeba poczekać, aż określone zadanie zakończy pracę.
	// W tym celu można użyć:
	// - metody Wait w obiekcie klasy Task (task.Wait())
	// - statycznej metody WaitAll lub WaitAny, podając im tablicę zadań.
	Task.WaitAll(taskA, taskB, taskC);
	stopwatch.Stop();
	WriteLine($"\nUpłynęło {stopwatch.ElapsedMilliseconds:#,##0}ms.");
}

static void TestMethod3()
{
	WriteThreadDetails();
	WriteLine("Przekazywanie wyniku jednej metody na wejście kolejnej.");
	Stopwatch stopwatch = Stopwatch.StartNew();

	var callWebsiteThenDataBase = Task.Factory
		.StartNew(MethodSimulatingWebsiteCall) // zwraca wartość typu Task<decimal>
	    .ContinueWith(previousTask => MethodSimulatingSqlQuery(previousTask.Result)); // zwraca wartość typu Task<string>

	WriteLine($"Wynik: {callWebsiteThenDataBase.Result}");
	stopwatch.Stop();
	WriteLine($"Upłynęło {stopwatch.ElapsedMilliseconds:#,##0}ms.");
}

static void WriteThreadDetails()
{
	Thread thread = Thread.CurrentThread;

	WriteLine("Id wątku: {0}, Priorytet: {1}, W tle: {2}, Nazwa: {3}, ThreadState: {4},",
	  thread.ManagedThreadId,
	  thread.Priority,
	  thread.IsBackground,
	  thread.Name ?? "null",
	  thread.ThreadState);

	//WriteLine("Id wątku: {0}, Priorytet: {1}, W tle: {2}, Nazwa: {3}, ThreadState: {4}, ExecutionContext: {5}, ApartmentState: {6}",
	//	thread.ManagedThreadId,
	//	thread.Priority,
	//	thread.IsBackground,
	//	thread.Name ?? "null",
	//	thread.ThreadState,
	//	thread.ExecutionContext,
	//	thread.GetApartmentState());
}

static void MethodSimulatingWorkA()
{
	WriteLine("Uruchomienie metody A...");
	WriteThreadDetails();
	Thread.Sleep(3000); // symulowanie pracy przez 3 sekundy
	WriteLine("Zakończenie metody A.");
}

static void MethodSimulatingWorkB()
{
	WriteLine("Uruchomienie metody B...");
	WriteThreadDetails();
	Thread.Sleep(2000); // symulowanie pracy przez 2 sekundy
	WriteLine("Zakończenie metody B.");
}

static void MethodSimulatingWorkC()
{
	WriteLine("Uruchomienie metody C...");
	WriteThreadDetails();
	Thread.Sleep(1000); // symulowanie pracy przez 1 sekundę
	WriteLine("Zakończenie metody C.");
}

static decimal MethodSimulatingWebsiteCall()
{
	WriteLine("Uruchamiam wywołanie serwisu WWW...");
	WriteThreadDetails();
	Thread.Sleep((new Random()).Next(2000, 4000));
	WriteLine("Zakończone wywołanie serwisu WWW.");
	return 89.99M;
}

static string MethodSimulatingSqlQuery(decimal amount)
{
	WriteLine("Uruchamiam wywołanie procedury osadzonej...");
	WriteThreadDetails();
	Thread.Sleep((new Random()).Next(2000, 4000));
	WriteLine("Zakończone wywołanie procedury osadzonej.");
	return $"12 produktów kosztuje więcej niż {amount:C}.";
}