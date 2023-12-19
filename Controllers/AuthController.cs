using System.Security.Claims;
using Eshopping_MVC.Data;
using Eshopping_MVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Eshopping_MVC.Controllers
{
    [Controller]
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AppDbContext context, ILogger<AuthController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Client client)
        {
            if (string.IsNullOrEmpty(client.email) || string.IsNullOrEmpty(client.password))
            {
                ModelState.AddModelError(string.Empty, "Email and password are required.");
                return View();
            }
            else if (client.email == "azer" && client.password == "azer")
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier,client.email),
                    new Claim("Role","User")
                };
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = true
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);
                return RedirectToAction("Index", "Home");
            }
            var dbClient = _context.Clients.FirstOrDefault(c => c.email == client.email && c.password == client.password);
            if (dbClient != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else ModelState.AddModelError(string.Empty, "Invalid username or password");
            return View();
        }
    }
}
