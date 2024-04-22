using System.Diagnostics;
using Microsoft.Extensions.Configuration;

// Celowo wprowadzony błąd!
static double Dodaj(double a, double b) 
	=> a + b;

double a = 4.5; // możesz też użyć słowa kluczowego var
double b = 2.5;
double wynik = Dodaj(a, b);
Console.WriteLine($"{a} + {b} = {wynik}");

// Zapisuj do pliku tekstowego na pulpicie.
Trace.Listeners.Add(new TextWriterTraceListener(
  File.CreateText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "log.txt"))));

// Zapisywanie tekstu jest buforowane, a ta opcja wywołuje
// metodę Flush() we wszystkich obiektach nasłuchujących po wykonaniu każdego zapisu.
Trace.AutoFlush = true;

Debug.WriteLine("Typ Debug mówi, że już jest gotowy!"); // działa tylko w Debug
Trace.WriteLine("Typ Trace mówi, że już jest gotowy!"); // działa w Debug i Release

ConfigurationBuilder tworcaKonfiguracji = new();

tworcaKonfiguracji.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

IConfigurationRoot konfiguracja = tworcaKonfiguracji.Build();

TraceSwitch ts = new(
  displayName: "Przelacznik",
  description: "Ten przełącznik jest ustawiany przez konfigurację z pliku JSON.");

konfiguracja.GetSection("Przelacznik").Bind(ts);

Trace.WriteLineIf(ts.TraceError, "Poziom błędów");
Trace.WriteLineIf(ts.TraceWarning, "Poziom ostrzeżeń");
Trace.WriteLineIf(ts.TraceInfo, "Poziom informacji");
Trace.WriteLineIf(ts.TraceVerbose, "Poziom ogólny");

Console.WriteLine("Naciśnij ENTER, aby zakończyć program.");
Console.ReadLine(); // czekaj, aż użytkownik naciśnie ENTER