namespace Library;

public struct DisplacementVector(int startX, int startY)
{
	public int X = startX;
	public int Y = startY;

	public static DisplacementVector operator +(DisplacementVector vector1, DisplacementVector vector2)
		=> new(vector1.X + vector2.X, vector1.Y + vector2.Y);
}