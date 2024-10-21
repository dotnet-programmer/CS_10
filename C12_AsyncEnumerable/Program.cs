await foreach (var number in GetNumbersAsync())
{
	Console.WriteLine($"Liczba: {number}");
}

async static IAsyncEnumerable<int> GetNumbersAsync()
{
	Random random = new();

	// symulowanie pracy
	await Task.Delay(random.Next(1000, 2000));
	yield return random.Next(0, 101);

	await Task.Delay(random.Next(1000, 2000));
	yield return random.Next(0, 101);

	await Task.Delay(random.Next(1000, 2000));
	yield return random.Next(0, 101);
}