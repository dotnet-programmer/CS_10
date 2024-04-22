using static System.Console;

namespace Library;

public class OdtwarzaczDVD : IOdtwarzacz
{
	public void Odtwarzaj()
		=> WriteLine("Odtwarzacz DVD odtwarza.");

	public void Pauza()
		=> WriteLine("Odtwarzacz DVD jest wstrzymany.");
}