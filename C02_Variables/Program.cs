object wysokosc = 1.88; // zapisanie wartości typu double
object imie = "Piotr";  // zapisanie wartości typu string
Console.WriteLine($"{imie} ma {wysokosc} wzrostu.");

//int dlugosc1 = imie.Length; // powoduje błąd kompilacji!
int dlugosc2 = ((string)imie).Length; // poinformuj kompilator, że pracuje z typem string
Console.WriteLine($"{imie} ma {dlugosc2} znaków.");
int asd = int.Parse("4");

// przechowywanie wartości typu string w zmiennej typu dynamic
// typ string ma właściwość Length
dynamic cos = "Paweł";

// typ int nie ma właściwości Length
// cos = 12;

// Tablica dowolnego typu ma właściwość Length
// cos = new[] { 3, 5, 7 };

// tę instrukcję można skompilować, ale może ona powodować błędy w czasie pracy programu,
// jeżeli wartość zapisana w zmiennej nie ma właściwości o nazwie Length!
Console.WriteLine($"Długość to {cos.Length}");

Console.WriteLine($"default(int) = {default(int)}");
Console.WriteLine($"default(bool) = {default(bool)}");
Console.WriteLine($"default(DateTime) = {default(DateTime)}");
Console.WriteLine($"default(string) = {default(string)}");

Console.Write("Naciśnij dowolną kombinację klawiszy: ");
ConsoleKeyInfo klawisz = Console.ReadKey();
Console.WriteLine();
Console.WriteLine("Klawisz: {0}, Znak: {1}, Modyfikatory: {2}",
	arg0: klawisz.Key,
	arg1: klawisz.KeyChar,
	arg2: klawisz.Modifiers);