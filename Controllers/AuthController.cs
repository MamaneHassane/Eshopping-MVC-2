using System.Security.Claims;
using Eshopping_MVC.Data;
using Eshopping_MVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Eshopping_MVC.Controllers
{
    [Controller]
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AuthController> _logger;
        private readonly IMemoryCache _cache;

        public AuthController(AppDbContext context, IMemoryCache cache, ILogger<AuthController> logger)
        {
            _context = context;
            _cache = cache;
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
            else if (client.email == "hassane" && client.password == "shopadmin1")
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, client.email),
                    new Claim("Role", "User")
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

            var dbClient =
                _context.Clients.FirstOrDefault(c => c.email == client.email && c.password == client.password);
            if (dbClient != null)
            {
                _cache.Set<int>("ClientId", dbClient.clientId);
                _logger.LogInformation("Client authentifié");

                if (dbClient.Cart == null)
                {
                    var existingCart = _context.Carts.FirstOrDefault(c => c.ClientId == dbClient.clientId);

                    if (existingCart == null)
                    {
                        var newCart = new Cart();
                        dbClient.Cart = newCart;

                        try
                        {
                            _context.Carts.Add(newCart);
                            _context.SaveChanges(); 
                        }
                        catch (DbUpdateException ex)
                        {
                            _logger.LogError(ex, "Erreur lors de l'enregistrement du panier dans la base de données");
                            throw;
                        }
                    }
                    else
                    {
                        dbClient.Cart = existingCart;
                    }
                }

                return RedirectToAction("ProductListForClient", "Cart");
            }
            else ModelState.AddModelError(string.Empty, "Invalid username or password");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var role = User.FindFirst("Role")?.Value;

            if (role == "User")
            {
                _cache.Remove("ClientId");
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
    }


}    
