namespace Library;

public interface IOdtwarzacz
{
	void Odtwarzaj();

	void Pauza();

	void Stop() // domyślna implementacja interfejsu
=> Console.WriteLine("Domyślna implementacja metody Stop");
}