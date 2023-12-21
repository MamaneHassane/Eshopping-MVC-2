using Microsoft.AspNetCore.Mvc;
using Eshopping_MVC.Data;
using Eshopping_MVC.Models;
using Microsoft.EntityFrameworkCore;

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
            return RedirectToAction("Index", "Home"); 
        }
        return View(model);
    }
    public IActionResult Index()
    {
        var clients = _context.Clients.ToList();
        return View(clients);
    }

    public IActionResult Edit(int id)
    {
        var client = _context.Clients.Find(id);

        if (client == null)
        {
            return NotFound();
        }

        return View(client);
    }

    [HttpPost]
    public IActionResult Edit(int id, Client model)
    {
        if (id != model.clientId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(model);
    }

    public IActionResult Delete(int id)
    {
        var client = _context.Clients.Find(id);

        if (client == null)
        {
            return NotFound();
        }

        return View(client);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var client = _context.Clients.Find(id);

        if (client == null)
        {
            return NotFound();
        }
        if (client.Cart != null)
        {
            _context.Carts.Remove(client.Cart);
        }

        _context.Clients.Remove(client);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}
