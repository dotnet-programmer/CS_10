// Projekt ASP.NET Core jest podobny do aplikacji najwy¿szego poziomu.
// Oznacza, ¿e ma ukryt¹ metodê Main, bêd¹c¹ punktem wejœcia dla programu, który otrzymuje parametr o nazwie args.

// WebApplication.CreateBuilder tworzy hosta dla witryny, stosuj¹c przy tym domyœlne ustawienia
//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

// Witryna bêdzie reagowaæ na ¿¹dania GET, odsy³aj¹c prosty tekst Hello World!
//app.MapGet("/", () => "Hello World!");

//// W³¹czanie silniejszych zabezpieczeñ i przekierowywanie na zabezpieczone po³¹czenie
//if (!app.Environment.IsDevelopment())
//{
//	// HSTS (ang. HTTP Strict Transport Security) jest opcjonalnym mechanizmem zabezpieczaj¹cym.
//	// Je¿eli witryna nakazuje u¿ycie tego mechanizmu, a przegl¹darka go obs³uguje,
//	// wówczas wymusza to stosowanie komunikacji z protoko³em HTTSP i uniemo¿liwia u¿ytkownikom
//	// korzystanie z niezaufanych i niepoprawnych certyfikatów.
//	app.UseHsts();
//}
// instrukcja przekierowuj¹ca ¿¹dania HTTP na protokó³ HTTPS
//app.UseHttpsRedirection();

// Wywo³ywana metoda Run jest metod¹ blokuj¹c¹, a zatem ukryta metoda Main nie koñczy siê do momentu zatrzymania serwera WWW
//app.Run();

// *********************************************************************************************************

using Northwind.Web; // klasa Startup

// Okreœlenie, ¿e podczas uruchamiania aplikacji bêdzie u¿yta klasa Startup.
Host.CreateDefaultBuilder(args)
	.ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
	.Build()
	.Run();

Console.WriteLine($"{Environment.NewLine}Ta instrukcja jest wykonywana po zatrzymaniu serwera!{Environment.NewLine}");