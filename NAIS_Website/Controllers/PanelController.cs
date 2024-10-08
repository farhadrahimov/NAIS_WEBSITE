using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace NAIS_Website.Controllers
{
    public class PanelController : Controller
    {
        private readonly string _userName;
        private readonly string _hashedPassword;
        private readonly IPasswordHasher<object> _passwordHasher;

        public PanelController(IConfiguration configuration, IPasswordHasher<object> passwordHasher)
        {
            _userName = configuration["StaticUser:UserName"];
            _hashedPassword = configuration["StaticUser:Password"];
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {            
            if (username == _userName && _passwordHasher.VerifyHashedPassword(null, _hashedPassword, password) == PasswordVerificationResult.Success)
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
