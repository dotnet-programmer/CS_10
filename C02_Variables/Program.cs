object height = 1.88; // zapisanie wartości typu double
object name = "Piotr";  // zapisanie wartości typu string
Console.WriteLine($"{name} ma {height} wzrostu.");

//int length = imie.Length; // powoduje błąd kompilacji!
int length = ((string)name).Length; // poinformuj kompilator, że pracuje z typem string
Console.WriteLine($"{name} ma {length} znaków.");
int number = int.Parse("4");

// przechowywanie wartości typu string w zmiennej typu dynamic
// typ string ma właściwość Length
dynamic dynamicVariable = "Paweł";

// typ int nie ma właściwości Length
// dynamicVariable = 12;

// Tablica dowolnego typu ma właściwość Length
// dynamicVariable = new[] { 3, 5, 7 };

// tę instrukcję można skompilować, ale może ona powodować błędy w czasie pracy programu,
// jeżeli wartość zapisana w zmiennej nie ma właściwości o nazwie Length!
Console.WriteLine($"Długość to {dynamicVariable.Length}");

Console.WriteLine($"default(int) = {default(int)}");
Console.WriteLine($"default(bool) = {default(bool)}");
Console.WriteLine($"default(DateTime) = {default(DateTime)}");
Console.WriteLine($"default(string) = {default(string)}");

Console.Write("Naciśnij dowolną kombinację klawiszy: ");
ConsoleKeyInfo key = Console.ReadKey();
Console.WriteLine();
Console.WriteLine("Klawisz: {0}, Znak: {1}, Modyfikatory: {2}",
	arg0: key.Key,
	arg1: key.KeyChar,
	arg2: key.Modifiers);