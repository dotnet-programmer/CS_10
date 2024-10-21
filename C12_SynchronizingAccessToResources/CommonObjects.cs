namespace C12_SynchronizingAccessToResources;

internal class CommonObjects
{
	public static Random Random = new();

	public static string Message; // wspólny zasób

	public static readonly object LockObject = new();

	public static int Licznik; // kolejny wspólny zasób
}