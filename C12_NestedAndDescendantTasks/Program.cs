// Zadania zagnieżdżone i potomne
// Oprócz definiowania zależności pomiędzy zadaniami możemy też definiować zadania zagnieżdżone i potomne.
// Zadanie zagnieżdżone jest zadaniem utworzonym przez inne zadanie.
// Zadanie potomne to zadanie zagnieżdżone, które musi się zakończyć, żeby zadanie nadrzędne również mogło zostać zakończone.

using static System.Console;

var externalMethod = Task.Factory.StartNew(ExternalMethod);
externalMethod.Wait();
WriteLine("Koniec pracy aplikacji konsoli.");
ReadLine();

static void ExternalMethod()
{
	WriteLine("Uruchamiam metodę zewnętrzną...");
	Task internalMethod = Task.Factory.StartNew(InternalMethod, TaskCreationOptions.AttachedToParent);
	WriteLine("Metoda zewnętrzna zakończona.");
}

static void InternalMethod()
{
	WriteLine("Uruchamiam metodę wewnętrzną...");
	Thread.Sleep(2000);
	WriteLine("Metoda wewnętrzna zakończona.");
}