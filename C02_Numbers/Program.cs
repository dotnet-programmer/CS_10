uint integerNumber = 23;
int integerNumberWithSign = -23;
float SinglePrecisionFloatingPointNumber = 2.3F;
double DoublePrecisionFloatingPointNumber = 2.3D;

Console.WriteLine(integerNumber);
Console.WriteLine(integerNumberWithSign);
Console.WriteLine(SinglePrecisionFloatingPointNumber);
Console.WriteLine(DoublePrecisionFloatingPointNumber);

int decimalNotation = 2_000_000;
int binaryNotation = 0b_0001_1110_1000_0100_1000_0000;
int hexadecimalNotation = 0x_001E_8480;

Console.WriteLine($"{decimalNotation == binaryNotation}");
Console.WriteLine($"{decimalNotation == hexadecimalNotation}");

Console.WriteLine($"Typ int zajmuje {sizeof(int)} bajtów i może przechowywać liczby z zakresu od {int.MinValue:N0} do {int.MaxValue:N0}.\n");
Console.WriteLine($"Typ double zajmuje {sizeof(double)} bajtów i może przechowywać liczby z zakresu od {double.MinValue} do {double.MaxValue}.\n");
Console.WriteLine($"Typ decimal zajmuje {sizeof(decimal)} bajtów i może  przechowywać liczby z zakresu od {decimal.MinValue} do {decimal.MaxValue}.\n");

Console.WriteLine("Używanie liczb typu double:");

double a = 0.1;
double b = 0.2;

if (a + b == 0.3)
{
	Console.WriteLine($"{a} + {b} jest równe 0,3");
}
else
{
	Console.WriteLine($"{a} + {b} NIE jest równe 0,3");
}

Console.WriteLine("Używanie liczb typu decimal:");
// M oznacza literał wartości dziesiętnej
decimal c = 0.1M;
decimal d = 0.2M;

if (c + d == 0.3M)
{
	Console.WriteLine($"{c} + {d} jest równe 0.3");
}
else
{
	Console.WriteLine($"{c} + {d} NIE jest równe 0.3");
}