static int FibonacciImperative(int position)
	=> position == 1
		? 0
		: position == 2
			? 1
			: FibonacciImperative(position - 1) + FibonacciImperative(position - 2);

static void RunFibonacciImperative()
{
	for (int i = 1; i <= 30; i++)
	{
		Console.WriteLine($"{i}. pozycja w ciągu Fibonacciego to {FibonacciImperative(i):N0}.");
	}
}

RunFibonacciImperative();

static int FibonacciFunctional(int position)
	=> position switch
	{
		1 => 0,
		2 => 1,
		_ => FibonacciFunctional(position - 1) + FibonacciFunctional(position - 2)
	};

static void RunFibonacciFunctional()
{
	for (int i = 1; i <= 30; i++)
	{
		Console.WriteLine($"{i}. pozycja w ciągu Fibonacciego to {FibonacciFunctional(i):N0}.");
	}
}

Console.WriteLine();

RunFibonacciFunctional();

Console.WriteLine();

static int Factorial(int number)
{
	if (number < 1)
	{
		return 0;
	}
	else if (number == 1)
	{
		return 1;
	}
	else
	{
		// wykrywanie przepełnienia
		checked
		{
			return number * Factorial(number - 1);
		}
	}
}

static int Factorial2(int number)
	=> number switch
	{
		< 1 => 0,
		1 => 1,
		_ => checked(number * Factorial2(number - 1))
	};

static void CalculateFactorial()
{
	for (int i = 1; i < 15; i++)
	{
		try
		{
			Console.WriteLine($"{i}! = {Factorial2(i):N0}");
		}
		catch (OverflowException)
		{
			Console.WriteLine($"{i}! jest zbyt wielka dla 32-bitowej zmiennej.");
		}
	}
}

CalculateFactorial();