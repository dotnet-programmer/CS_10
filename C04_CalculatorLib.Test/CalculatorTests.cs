namespace CalculatorLib.Test;

public class CalculatorTests
{
	[Fact]
	public void TestAdd2and2()
	{
		// 3xA:
		// przygotuj - Arrange
		double a = 2;
		double b = 2;
		double expectedResult = 4;
		Calculator calculator = new();

		// wykonaj - Act
		double result = calculator.Add(a, b);

		// sprawdü - Assert
		Assert.Equal(expectedResult, result);
	}

	[Fact]
	public void TestAdd2and3()
	{
		// przygotuj
		double a = 2;
		double b = 3;
		double expectedResult = 5;
		Calculator calculator = new();

		// wykonaj
		double result = calculator.Add(a, b);

		// sprawdü
		Assert.Equal(expectedResult, result);
	}
}