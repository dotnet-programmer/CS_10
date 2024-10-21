using BenchmarkDotNet.Attributes; // [Benchmark]

namespace C12_MonitoringLibrary;

public class StringMeasurements
{
	private readonly int[] _numbers;

	public StringMeasurements()
		=> _numbers = Enumerable.Range(start: 1, count: 20).ToArray();

	[Benchmark(Baseline = true)]
	public string TestString()
	{
		string s = string.Empty;
		for (int i = 0; i < _numbers.Length; i++)
		{
			s += _numbers[i] + ", ";
		}
		return s;
	}

	[Benchmark]
	public string TestStringBuilder()
	{
		System.Text.StringBuilder sb = new();
		for (int i = 0; i < _numbers.Length; i++)
		{
			sb.Append(_numbers[i]);
			sb.Append(", ");
		}
		return sb.ToString();
	}
}