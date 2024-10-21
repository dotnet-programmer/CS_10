using System.Text;
using BenchmarkDotNet.Running;
using C12_MonitoringLibrary;

Console.WriteLine("Testowe uruchomienie klasy Recording: \n");
RecordingClassTest();
Console.ReadLine();

Console.WriteLine("Pomiar wydajności łączenia string i StringBuilder klasą Record:");
MeasuringEfficiencyOfStrings();
Console.ReadLine();

Console.WriteLine("Pomiar wydajności łączenia string i StringBuilder biblioteką BenchmarkDotNet:");
BenchmarkRunner.Run<StringMeasurements>();
Console.ReadLine();

static void RecordingClassTest()
{
	Console.WriteLine("Przetwarzanie, proszę czekać…");

	Recording.Start();

	// Symulowanie procesu wymagającego sporej ilości pamięci…
	int[] bigArray = Enumerable.Range(start: 1, count: 10_000).ToArray();

	// …i zajmującego sporą ilość czasu.
	Thread.Sleep(new Random().Next(5, 10) * 1000);

	Recording.Stop();
}

static void MeasuringEfficiencyOfStrings()
{
	int[] numbers = Enumerable.Range(start: 1, count: 50000).ToArray();

	Console.WriteLine("\nUżywam klasy string i operatora +");
	Recording.Start();
	string s = string.Empty;
	for (int i = 0; i < numbers.Length; i++)
	{
		s += numbers[i] + ", ";
	}
	Recording.Stop();

	Console.WriteLine("\nUżywam klasy StringBuilder");
	Recording.Start();
	StringBuilder sb = new();
	for (int i = 0; i < numbers.Length; i++)
	{
		sb.Append(numbers[i]);
		sb.Append(", ");
	}
	Recording.Stop();
}