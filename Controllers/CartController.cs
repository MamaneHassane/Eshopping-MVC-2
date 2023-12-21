using Microsoft.Extensions.Caching.Memory;

namespace Eshopping_MVC.Controllers;

using Eshopping_MVC.Data;
using Eshopping_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class CartController : Controller
{
    private readonly AppDbContext _context;
    private readonly IMemoryCache _cache;
    public CartController(AppDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public IActionResult Index()
    {
        var clientId = GetCurrentClientId();
        var clientWithCart = _context.Clients.Include(c => c.Cart).ThenInclude(cart => cart.Products).FirstOrDefault(c => c.clientId == clientId);

        if (clientWithCart == null)
        {
            return View("Error"); 
        }

        var cart = clientWithCart.Cart;

        if (cart == null || cart.Products == null || cart.Products.Count == 0)
        {
            ViewBag.IsEmptyCart = true;
            return View();
        }
        return View(cart.Products);
    }
    
    public IActionResult ProductListForClient()
    {
        var products = _context.Products.ToList();
        return View(products);
    }

    public IActionResult AddToCart(int productId)
    {
        var clientId = GetCurrentClientId();
        var client = _context.Clients.Include(c => c.Cart).FirstOrDefault(c => c.clientId == clientId);

        if (client == null)
        {
            return View("Error");
        }

        if (client.Cart == null)
        {
            var newCart = new Cart();
            client.Cart = newCart;
            _context.Carts.Add(newCart);
        }

        var cart = client.Cart;
        var product = _context.Products.Find(productId);

        if (product != null)
        {
            if (product.Quantity == 1) product.Quantity++;
            else product.Quantity = 1; 
            if(product.Quantity==1) cart.Products.Add(product);
        }
        
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    public IActionResult RemoveFromCart(int productId)
    {
        var clientId = GetCurrentClientId();
        var cart = _context.Carts.Include(c => c.Products).FirstOrDefault(c => c.ClientId == clientId);

        if (cart == null)
        {
            return View("Error");
        }

        var productToRemove = cart.Products.FirstOrDefault(p => p.productId == productId);
        if (productToRemove != null)
        {
            productToRemove.Quantity = 0;
        }
        return RedirectToAction("Index");
    }


    
    [HttpGet]
    public IActionResult DeleteCart()
    {
        var role = User.FindFirst("Role")?.Value;

        if (role == "User")
        {
            _cache.Remove("ClientId");
        }

        var clientId = GetCurrentClientId();
        var client = _context.Clients.Include(c => c.Cart).FirstOrDefault(c => c.clientId == clientId);

        if (client != null && client.Cart != null)
        {
            _context.Clients.FirstOrDefault(c => c.clientId == client.clientId).Cart = new Cart();
            _context.SaveChanges();
        }

        return RedirectToAction("ProductListForClient", "Cart");
    }

    public IActionResult Checkout()
    {
        var clientId = GetCurrentClientId();
        var client = _context.Clients.Include(c => c.Cart).FirstOrDefault(c => c.clientId == clientId);

        if (client == null)
        {
            return View("Error");
        }

        var cart = client.Cart;
        

        cart.Products.Clear();
        _context.SaveChanges();

        return View("ThankYou"); 
    }
    
    private int GetCurrentClientId()
    {
        if (_cache.TryGetValue<int>("ClientId", out var clientId))
        {
            return clientId;
        }

        return 0; 
    }
}