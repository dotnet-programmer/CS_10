namespace Library;

public struct WektorPrzesuniecia
{
	public int X;
	public int Y;

	public WektorPrzesuniecia(int poczatkoweX, int poczatkoweY)
	{
		X = poczatkoweX;
		Y = poczatkoweY;
	}

	public static WektorPrzesuniecia operator +(WektorPrzesuniecia wektor1, WektorPrzesuniecia wektor2) => new(wektor1.X + wektor2.X, wektor1.Y + wektor2.Y);
}