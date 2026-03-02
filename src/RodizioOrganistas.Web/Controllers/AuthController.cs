using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RodizioOrganistas.Domain.Interfaces;
using RodizioOrganistas.Web.Models;

namespace RodizioOrganistas.Web.Controllers;

public class AuthController(IUserRepository userRepository) : Controller
{
    [HttpGet]
    public IActionResult Login() => View(new LoginViewModel());

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await userRepository.GetByUsernameAsync(model.Username);
        if (user is null || user.Password != model.Password)
        {
            ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Role, user.Role.ToString())
        };
        if (user.ChurchId.HasValue) claims.Add(new("ChurchId", user.ChurchId.Value.ToString()));

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        return RedirectToAction("Index", "Schedules");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }
}
