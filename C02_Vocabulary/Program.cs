using System.Reflection;

double heightInMeters = 1.88;
Console.WriteLine($"Variable {nameof(heightInMeters)} = {heightInMeters}");

// deklarowanie nieużywanych zmiennych za pomocą typów z zewnętrznych zestawów
System.Data.DataSet dataSet;
HttpClient httpClient;

// refleksja / odzwierciedlanie
Assembly? assembly = Assembly.GetEntryAssembly();
if (assembly == null)
{
	return;
}

// przejrzyj w pętli zestawy, do których odwołuje się aplikacja
foreach (AssemblyName item in assembly.GetReferencedAssemblies())
{
	// załaduj zestaw, żeby można było odczytać szczegóły na jego temat
	var tmpAssembly = Assembly.Load(item);

	// zadeklaruj zmienną zliczającą ogólną liczbę metod
	int methodsCount = 0;

	// przejrzyj w pętli wszystkie typy z zestawu
	foreach (var type in tmpAssembly.DefinedTypes)
	{
		// zsumuj liczbę metod
		methodsCount += type.GetMethods().Count();
	}

	// wypisz liczbę typów i ich metod
	Console.WriteLine($"W zestawie {item.Name} jest {tmpAssembly.DefinedTypes.Count()} typów i {methodsCount} metod");
}