using System.Text.RegularExpressions;

namespace Library;

public static class RozszerzeniaDlaString
{
	// użyj prostego wyrażenia regularnego, żeby skontrolować poprawność adresu e-mail
	public static bool AdresPoprawny(this string wejscie)
		=> Regex.IsMatch(wejscie, @"[a-zA-Z0-9\.-_]+@[a-zA-Z0-9\.-_]+");
}