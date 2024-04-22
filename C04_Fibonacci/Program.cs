static int FibImperatywna(int pozycja)
	=> pozycja == 1
		? 0
		: pozycja == 2
			? 1
			: FibImperatywna(pozycja - 1) + FibImperatywna(pozycja - 2);

static void UruchomFibImperatywna()
{
	for (int i = 1; i <= 30; i++)
	{
		Console.WriteLine("{0}. pozycja w ciągu Fibonacciego to {1:N0}.",
		  arg0: i,
		  arg1: FibImperatywna(pozycja: i));
	}
}

UruchomFibImperatywna();

static int FibFunkcyjna(int pozycja)
	=> pozycja switch
	{
		1 => 0,
		2 => 1,
		_ => FibFunkcyjna(pozycja - 1) + FibFunkcyjna(pozycja - 2)
	};

static void UruchomFibFunkcyjna()
{
	for (int i = 1; i <= 30; i++)
	{
		Console.WriteLine("{0}. pozycja w ciągu Fibonacciego to {1:N0}.",
		  arg0: i,
		  arg1: FibFunkcyjna(pozycja: i));
	}
}

Console.WriteLine();

UruchomFibFunkcyjna();