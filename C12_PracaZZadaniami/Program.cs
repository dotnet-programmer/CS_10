using System.Diagnostics; // Stopwatch
using static System.Console;

TestMethod1();
Console.WriteLine();
TestMethod2();
Console.WriteLine();
TestMethod3();

static void TestMethod1()
{
	WypiszDaneWatku();
	WriteLine("Uruchamianie metod synchronicznie, w jednym wątku.");
	Stopwatch stoper = Stopwatch.StartNew();
	MetodaA();
	MetodaB();
	MetodaC();
	WriteLine($"Upłynęło {stoper.ElapsedMilliseconds:#,##0}ms.");
}

static void TestMethod2()
{
	WypiszDaneWatku();
	WriteLine("Uruchamianie metod asynchroniczne, w wielu wątkach.");
	Stopwatch stoper = Stopwatch.StartNew();
	Task zadanieA = new(MetodaA);
	zadanieA.Start();
	Task zadanieB = Task.Factory.StartNew(MetodaB);
	Task zadanieC = Task.Run(MetodaC);
	Task.WaitAll(zadanieA, zadanieB, zadanieC);
	WriteLine($"Upłynęło {stoper.ElapsedMilliseconds:#,##0}ms.");
}

static void TestMethod3()
{
	WypiszDaneWatku();
	WriteLine("Przekazywanie wyniku jednej metody na wejście kolejnej.");
	Stopwatch stoper = Stopwatch.StartNew();

	var zadanieWywolajSerwisWWWaPotemBazeDanych = Task.Factory.StartNew(WywolajSerwisWWW)
	  .ContinueWith(poprzednieZadanie => WywolajProcedureOsadzona(poprzednieZadanie.Result));

	WriteLine($"Wynik: {zadanieWywolajSerwisWWWaPotemBazeDanych.Result}");
	WriteLine($"Upłynęło {stoper.ElapsedMilliseconds:#,##0}ms.");
}

static void WypiszDaneWatku()
{
	Thread t = Thread.CurrentThread;

	WriteLine("Id wątku: {0}, Priorytet: {1}, W tle: {2}, Nazwa: {3}, ThreadState: {4}, ExecutionContext: {5}, ApartmentState: {6}",
	  t.ManagedThreadId,
	  t.Priority,
	  t.IsBackground,
	  t.Name ?? "null",
	  t.ThreadState,
	  t.ExecutionContext,
	  t.ApartmentState);
}

static void MetodaA()
{
	WriteLine("Uruchomienie metody A...");
	WypiszDaneWatku();
	Thread.Sleep(3000); // symulowanie pracy przez 3 sekundy
	WriteLine("Zakończenie metody A.");
}

static void MetodaB()
{
	WriteLine("Uruchomienie metody B...");
	WypiszDaneWatku();
	Thread.Sleep(2000); // symulowanie pracy przez 2 sekundy
	WriteLine("Zakończenie metody B.");
}

static void MetodaC()
{
	WriteLine("Uruchomienie metody C...");
	WypiszDaneWatku();
	Thread.Sleep(1000); // symulowanie pracy przez 1 sekundę
	WriteLine("Zakończenie metody C.");
}

static decimal WywolajSerwisWWW()
{
	WriteLine("Uruchamiam wywołanie serwisu WWW...");
	WypiszDaneWatku();
	Thread.Sleep((new Random()).Next(2000, 4000));
	WriteLine("Zakończone wywołanie serwisu WWW.");
	return 89.99M;
}

static string WywolajProcedureOsadzona(decimal kwota)
{
	WriteLine("Uruchamiam wywołanie procedury osadzonej...");
	WypiszDaneWatku();
	Thread.Sleep((new Random()).Next(2000, 4000));
	WriteLine("Zakończone wywołanie procedury osadzonej.");
	return $"12 produktów kosztuje więcej niż {kwota:C}.";
}