namespace Library;

public interface IPlayer
{
	void Play();

	void Pause();

	// domyślna implementacja interfejsu
	void Stop()
		=> Console.WriteLine("Domyślna implementacja metody Stop");
}