using static System.Console;

namespace Library;

public class DvdPlayer : IPlayer
{
	public void Play()
		=> WriteLine("Odtwarzacz DVD odtwarza.");

	public void Pause()
		=> WriteLine("Odtwarzacz DVD jest wstrzymany.");
}