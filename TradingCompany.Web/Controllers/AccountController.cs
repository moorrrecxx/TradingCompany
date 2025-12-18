using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TradingCompany.BLL.Interfaces;
using TradingCompany.Web.Models;

namespace TradingCompany.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthManager _manager;

        public AccountController(IAuthManager manager)
        {
            _manager = manager;
        }

        public IActionResult Login(string? ReturnUrl)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            var model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string? ReturnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_manager.Login(model.Username, model.Password))
                    {
                        var user = _manager.GetUserByLogin(model.Username);
                        var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Username),
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    };

                        var PrivilegeType = _manager.GetRoleNameByLogin(model.Username);

                        if (PrivilegeType == "Admin")
                        {
                            claims.Add(new Claim(ClaimTypes.Role, nameof(PrivilegeType)));
                        }
                        else
                        {
                            claims.Add(new Claim(ClaimTypes.Role, nameof(PrivilegeType)));
                        }

                        var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            // AllowRefresh = <bool>,
                            // Refreshing the authentication session should be allowed.
                            // ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                            // The time at which the authentication ticket expires.
                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View(model);
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An exception has occurred: {ex.Message}");
                return View(model);
            }
        }

        public IActionResult Forbidden()
        {
            return View();
        }
    }
}

