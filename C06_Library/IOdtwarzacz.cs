namespace Library;

public interface IOdtwarzacz
{
	void Odtwarzaj();

	void Pauza();

	// domyślna implementacja interfejsu
	void Stop() 
		=> Console.WriteLine("Domyślna implementacja metody Stop");
}