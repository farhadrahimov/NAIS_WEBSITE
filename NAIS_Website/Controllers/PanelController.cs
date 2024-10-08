using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace NAIS_Website.Controllers
{
    public class PanelController : Controller
    {
        private const string Username = "Admin1";
        private const string Password = "Nais0102@";

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if(username == Username && password == Password)
            {
                var Claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,username)
                };

                var claimsIdentity = new ClaimsIdentity(Claims,CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("GetAll","Catalog");
            }

            ViewBag.Error = "Daxil etdiyiniz məlumatlar yalnışdır, yenidən cəhd edin";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return StatusCode(403);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
