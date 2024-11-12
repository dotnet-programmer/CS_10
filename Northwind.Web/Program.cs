// Projekt ASP.NET Core jest podobny do aplikacji najwy�szego poziomu.
// Oznacza, �e ma ukryt� metod� Main, b�d�c� punktem wej�cia dla programu, kt�ry otrzymuje parametr o nazwie args.

// WebApplication.CreateBuilder tworzy hosta�dla witryny, stosuj�c przy tym domy�lne ustawienia
//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

// Witryna b�dzie reagowa� na ��dania GET, odsy�aj�c prosty tekst Hello World!
//app.MapGet("/", () => "Hello World!");

//// W��czanie silniejszych zabezpiecze� i przekierowywanie na zabezpieczone po��czenie
//if (!app.Environment.IsDevelopment())
//{
//	// HSTS (ang. HTTP Strict Transport Security) jest opcjonalnym mechanizmem zabezpieczaj�cym.
//	// Je�eli witryna nakazuje u�ycie tego mechanizmu, a przegl�darka go obs�uguje,
//	// w�wczas wymusza to stosowanie komunikacji z protoko�em HTTSP i uniemo�liwia u�ytkownikom
//	// korzystanie z niezaufanych i niepoprawnych certyfikat�w.
//	app.UseHsts();
//}
// instrukcja przekierowuj�ca ��dania HTTP na protok� HTTPS
//app.UseHttpsRedirection();

// Wywo�ywana metoda Run jest metod� blokuj�c�, a zatem ukryta metoda Main nie ko�czy si� do momentu zatrzymania serwera WWW
//app.Run();

// *********************************************************************************************************

using Northwind.Web; // klasa Startup

// Okre�lenie, �e podczas uruchamiania aplikacji b�dzie u�yta klasa Startup.
Host.CreateDefaultBuilder(args)
	.ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
	.Build()
	.Run();

Console.WriteLine($"{Environment.NewLine}Ta instrukcja jest wykonywana po zatrzymaniu serwera!{Environment.NewLine}");