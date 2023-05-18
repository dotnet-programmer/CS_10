using System.Text.RegularExpressions;

namespace Library;

public static class RozszerzeniaDlaString
{
	public static bool AdresPoprawny(this string wejscie) =>
		// użyj prostego wyrażenia regularnego, żeby skontrolować poprawność adresu e-mail
		Regex.IsMatch(wejscie, @"[a-zA-Z0-9\.-_]+@[a-zA-Z0-9\.-_]+");
}