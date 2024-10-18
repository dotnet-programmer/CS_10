using System.Text.RegularExpressions;

namespace Library;

public static class StringExtension
{
	// użyj prostego wyrażenia regularnego, żeby skontrolować poprawność adresu e-mail
	public static bool CorrectEmail(this string input)
		=> Regex.IsMatch(input, @"[a-zA-Z0-9\.-_]+@[a-zA-Z0-9\.-_]+");
}