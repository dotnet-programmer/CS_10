using Microsoft.Extensions.Logging; // ILoggerProvider, ILogger, LogLevel

using static System.Console;

namespace BibliotekaWspolna;

public class DostawcaProtokoluKonsoli : ILoggerProvider
{
	// można tworzyć osobne implementacje dla różnych nazw kategorii, ale tutaj mamy tylko jedną
	public ILogger CreateLogger(string nazwaKategorii) => new ProtokolKonsoli();

	// jeżeli klasa protokołu używa zasobów niezarządzanych, to tutaj powinna je zwolnić
	public void Dispose()
	{ }
}

public class ProtokolKonsoli : ILogger
{
	// jeżeli klasa protokołu używa zasobów niezarządzanych, to tutaj możesz zwrócić klasę implementującą interfejs IDisposable
	public IDisposable BeginScope<TState>(TState state) => null;

	public bool IsEnabled(LogLevel logLevel)
	{
		// aby ograniczyć ilość protokołowanych informacji, możesz tutaj filtrować według poziomu protokołu
		return logLevel switch
		{
			LogLevel.Trace or LogLevel.Information or LogLevel.None => false,
			_ => true,
		};
		;
	}

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string> formatter)
	{
		// w jaki sposób zapytanie LINQ zostało przełożone na instrukcje SQL, to musielibyśmy poszukać zdarzenia o identyfikatorze 20100
		if (eventId.Id == 20100)
		{
			// wypisz poziom protokołu i identyfikator zdarzenia
			Write($"Poziom: {logLevel}, ID zdarzenia: {eventId}");

			// dane stanu lub wyjątku wypisz tylko w przypadku, gdy istnieją
			if (state != null)
			{
				Write($", Stan: {state}");
			}

			if (exception != null)
			{
				Write($", Wyjątek: {exception.Message}");
			}

			WriteLine();
		}
	}
}