using Microsoft.AspNetCore.Mvc;
using Eshopping_MVC.Data;
using Eshopping_MVC.Models;
namespace Eshopping_MVC.Controllers;

public class ClientController : Controller
{
    private readonly AppDbContext _context;

    public ClientController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(Client model)
    {
        if (ModelState.IsValid)
        {
            _context.Clients.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home"); // Rediriger vers la page d'authentification
        }
        // Cas d'erreur
        return View(model);
    }
}