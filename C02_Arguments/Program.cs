using static System.Console;

ForegroundColor = (ConsoleColor)Enum.Parse(
  enumType: typeof(ConsoleColor),
  value: args[0],
  ignoreCase: true);

BackgroundColor = (ConsoleColor)Enum.Parse(
  enumType: typeof(ConsoleColor),
  value: args[1],
  ignoreCase: true);

try
{
	CursorSize = int.Parse(args[2]);
}
catch (PlatformNotSupportedException ex)
{
	WriteLine($"Aktualna platforma nie pozwala na zmianę wielkości kursora.\n{ex.Message}");
}

if (OperatingSystem.IsWindows())
{
	// wykonaj kod działający wyłącznie w systemie Windows
}
else if (OperatingSystem.IsWindowsVersionAtLeast(major: 10))
{
	// wykonaj kod działający wyłącznie w systemie Windows 10 lub nowszym
}
else if (OperatingSystem.IsIOSVersionAtLeast(major: 14, minor: 5))
{
	// wykonaj kod działający wyłącznie w systemie iOS 14.5 lub nowszym
}
else if (OperatingSystem.IsBrowser())
{
	// wykonaj kod działający wyłącznie w przeglądarce z technologią Blazor
}

#if NET6_0_ANDROID
// kompiluj instrukcje działające w systemie Android
#elif NET6_0_IOS
// kompiluj instrukcje działające w systemie iOS
#else
// kompiluj instrukcje działające w pozostałych systemach
#endif

WriteLine($"Otrzymano {args.Length} argumentów.");

foreach (var item in args)
{
	WriteLine(item);
}

if (args.Length < 3)
{
	WriteLine("Musisz podać dwa kolory oraz wielkość kursora, np.:");
	WriteLine("dotnet run red yellow 50");
	return; // Zakończ pracę.
}