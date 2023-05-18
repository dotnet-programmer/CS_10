// See https://aka.ms/new-console-template for more information

using System.Reflection;

double heightInMeters = 1.88;
Console.WriteLine($"Variable {nameof(heightInMeters)} = {heightInMeters}");

// deklarowanie nieużywanych zmiennych za pomocą typów z zewnętrznych zestawów
System.Data.DataSet ds;
HttpClient client;

// refleksja / odzwierciedlanie
Assembly? zestaw = Assembly.GetEntryAssembly();
if (zestaw == null)
{
	return;
}

// przejrzyj w pętli zestawy, do których odwołuje się aplikacja
foreach (AssemblyName item in zestaw.GetReferencedAssemblies())
{
	// załaduj zestaw, żeby można było odczytać szczegóły na jego temat
	var tmpAssembly = Assembly.Load(item);

	// zadeklaruj zmienną zliczającą ogólną liczbę metod
	int methodsCount = 0;

	// przejrzyj w pętli wszystkie typy z zestawu
	foreach (var t in tmpAssembly.DefinedTypes)
	{
		// zsumuj liczbę metod
		methodsCount += t.GetMethods().Count();
	}

	// wypisz liczbę typów i ich metod
	Console.WriteLine($"W zestawie {item.Name} jest {tmpAssembly.DefinedTypes.Count()} typów i {methodsCount} metod");
}