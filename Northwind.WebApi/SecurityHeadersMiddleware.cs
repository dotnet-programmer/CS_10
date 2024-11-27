using Microsoft.Extensions.Primitives; // StringValues

namespace Northwind.WebApi;

// Dodawanie zabezpieczających nagłówków HTTP

public class SecurityHeaders
{
	private readonly RequestDelegate _next;

	public SecurityHeaders(RequestDelegate next)
	{
		_next = next;
	}

	public Task Invoke(HttpContext context)
	{
		// dodaj tutaj inne dowolne nagłówki HTTP
		context.Response.Headers.Add("super-secure", new StringValues("enable"));
		return _next(context);
	}
}