using static System.Console;

HttpClient klient = new();

HttpResponseMessage odpowiedz = await klient.GetAsync("http://www.wp.pl/");

WriteLine("Strona główna WP.PL składa się z {0:N0} bajtów.",
  odpowiedz.Content.Headers.ContentLength);