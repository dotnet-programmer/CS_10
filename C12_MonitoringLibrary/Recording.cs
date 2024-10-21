using System.Diagnostics; // Stopwatch, Process

namespace C12_MonitoringLibrary;

public class Recording
{
	private static readonly Stopwatch _stopwatch = new();
	private static long _physicalBytesBefore = 0;
	private static long _virtualBytesBefore = 0;

	public static void Start()
	{
		// Wymuszamy dwa cykle oczyszczania pamięci, aby zwolnić
		// nieużywaną już pamięć, która jeszcze nie została zwolniona.
		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();

		// Zapisujemy poziom wykorzystania pamięci fizycznej i wirtualnej.
		_physicalBytesBefore = Process.GetCurrentProcess().WorkingSet64;
		_virtualBytesBefore = Process.GetCurrentProcess().VirtualMemorySize64;
		_stopwatch.Restart();
	}

	public static void Stop()
	{
		_stopwatch.Stop();
		long physicalBytesAfter = Process.GetCurrentProcess().WorkingSet64;
		long virtualBytesAfter = Process.GetCurrentProcess().VirtualMemorySize64;

		Console.WriteLine("Wykorzystano {0:N0} fizycznych bajtów.",
		  physicalBytesAfter - _physicalBytesBefore);

		Console.WriteLine("Wykorzystano {0:N0} wirtualnych bajtów.",
		  virtualBytesAfter - _virtualBytesBefore);

		Console.WriteLine("Upłynęło {0} czasu.",
			_stopwatch.Elapsed);

		Console.WriteLine("Upłynęło {0:N0} milisekund.",
		  _stopwatch.ElapsedMilliseconds);
	}
}