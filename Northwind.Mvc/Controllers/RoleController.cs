using Microsoft.AspNetCore.Identity; // RoleManager, UserManager
using Microsoft.AspNetCore.Mvc; // Controller, IActionResult

namespace Northwind.Mvc.Controllers;
public class RoleController : Controller
{
	private readonly string _adminRole = "Administratorzy";
	private readonly string _userEmail = "test@przyklad.pl";

	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly UserManager<IdentityUser> _userManager;

	// Konstruktor pobiera i zachowuje zarejestrowane, zależne serwisy menedżera użytkowników i menedżera ról.
	public RoleController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
	{
		_roleManager = roleManager;
		_userManager = userManager;
	}

	public async Task<IActionResult> Index()
	{
		if (!(await _roleManager.RoleExistsAsync(_adminRole)))
		{
			await _roleManager.CreateAsync(new IdentityRole(_adminRole));
		}

		IdentityUser? user = await _userManager.FindByEmailAsync(_userEmail);
		if (user == null)
		{
			user = new()
			{
				UserName = _userEmail,
				Email = _userEmail
			};
			IdentityResult result = await _userManager.CreateAsync(user, "Ha$$l0");

			if (result.Succeeded)
			{
				Console.WriteLine($"Użytkownik {user.UserName} został utworzony.");
			}
			else
			{
				foreach (IdentityError error in result.Errors)
				{
					Console.WriteLine(error.Description);
				}
			}
		}

		if (!user.EmailConfirmed)
		{
			string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);

			if (result.Succeeded)
			{
				Console.WriteLine($"Adres e-mail użytkownika {user.UserName} został potwierdzony.");
			}
			else
			{
				foreach (IdentityError error in result.Errors)
				{
					Console.WriteLine(error.Description);
				}
			}
		}

		if (!(await _userManager.IsInRoleAsync(user, _adminRole)))
		{
			IdentityResult result = await _userManager.AddToRoleAsync(user, _adminRole);

			if (result.Succeeded)
			{
				Console.WriteLine($"Użytkownik {user.UserName} został dodany do roli {_adminRole}.");
			}
			else
			{
				foreach (IdentityError error in result.Errors)
				{
					Console.WriteLine(error.Description);
				}
			}
		}

		return Redirect("/");
	}
}