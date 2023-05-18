using BenchmarkDotNet.Attributes; // [Benchmark]

public class PomiaryString
{
	private readonly int[] liczby;

	public PomiaryString() => liczby = Enumerable.Range(start: 1, count: 20).ToArray();

	[Benchmark(Baseline = true)]
	public string TestLaczeniaCiagowZnakow()
	{
		string s = string.Empty;
		for (int i = 0; i < liczby.Length; i++)
		{
			s += liczby[i] + ", ";
		}
		return s;
	}

	[Benchmark]
	public string TestStringBuilder()
	{
		System.Text.StringBuilder sb = new();
		for (int i = 0; i < liczby.Length; i++)
		{
			sb.Append(liczby[i]);
			sb.Append(", ");
		}
		return sb.ToString();
	}
}