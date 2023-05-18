using static System.Console;

var zewnetrzna = Task.Factory.StartNew(MetodaZewnętrzna);
zewnetrzna.Wait();
WriteLine("Koniec pracy aplikacji konsoli.");

static void MetodaZewnętrzna()
{
	WriteLine("Uruchamiam metodę zewnętrzną...");
	Task wewnetrzna = Task.Factory.StartNew(MetodaWewnętrzna, TaskCreationOptions.AttachedToParent);
	WriteLine("Metoda zewnętrzna zakończona.");
}

static void MetodaWewnętrzna()
{
	WriteLine("Uruchamiam metodę wewnętrzną...");
	Thread.Sleep(2000);
	WriteLine("Metoda wewnętrzna zakończona.");
}