using MesaAyuda.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace MesaAyuda.Web.Controllers;
public class AccountController(SignInManager<AppUser> signIn) : Controller
{
    [HttpGet, AllowAnonymous]
    public IActionResult Login(string? returnUrl) => View(new LoginInput { ReturnUrl = returnUrl });
    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> Login(LoginInput input)
    {
        if (!ModelState.IsValid) return View(input);
        var result = await signIn.PasswordSignInAsync(input.Email.Trim(), input.Password, false, lockoutOnFailure: true);
        if (result.Succeeded)
            return Url.IsLocalUrl(input.ReturnUrl) ? LocalRedirect(input.ReturnUrl!) : RedirectToAction("Index", "Tickets");
        ModelState.AddModelError("", result.IsLockedOut
            ? "Demasiados intentos. Esperá cinco minutos antes de volver a ingresar."
            : "No pudimos iniciar sesión con esos datos.");
        return View(input);
    }
    [HttpPost, Authorize]
    public async Task<IActionResult> Logout()
    {
        await signIn.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }
    public IActionResult Denied() { Response.StatusCode = 403; return View(); }
}
