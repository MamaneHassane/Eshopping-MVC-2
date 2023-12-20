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
        // Retrieve the client's cart with products
        var clientId = GetCurrentClientId(); // Implement this method to get the current client ID
        var clientWithCart = _context.Clients.Include(c => c.Cart).ThenInclude(cart => cart.Products).FirstOrDefault(c => c.clientId == clientId);

        if (clientWithCart == null)
        {
            return View("Error"); // Handle the case where the client or cart is not found
        }
        var cart = clientWithCart.Cart;
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

        var cart = client.Cart;

        // Vérifier si le produit est déjà dans le panier
        var existingProduct = cart.Products.FirstOrDefault(p => p.productId == productId);

        if (existingProduct != null)
        {
            // Le produit est déjà dans le panier, vous pouvez mettre à jour la quantité ici
            existingProduct.Quantity++; // ou tout autre logique d'augmentation de la quantité
        }
        else
        {
            // Le produit n'est pas encore dans le panier, ajoutez-le
            var product = _context.Products.Find(productId);

            if (product != null)
            {
                cart.Products.Add(product);
            }
        }
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
    
    // Add other actions as needed (e.g., RemoveFromCart, Checkout, etc.)
    public IActionResult RemoveFromCart(int productId)
    {
        var clientId = GetCurrentClientId();
        var client = _context.Clients.Include(c => c.Cart).FirstOrDefault(c => c.clientId == clientId);
    
        if (client == null)
        {
            return View("Error");
        }
    
        var cart = client.Cart;
    
        // Vérifier si le produit est dans le panier avant de le supprimer
        var product = cart.Products.FirstOrDefault(p => p.productId == productId);
        if (product != null)
        {
            cart.Products.Remove(product);
            _context.SaveChanges(); // Sauvegarder uniquement si le produit est retiré du panier
        }
    
        return RedirectToAction("Index");
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
        
        // Implement checkout logic as needed

        // Clear the cart after checkout
        cart.Products.Clear();
        _context.SaveChanges();

        return View("ThankYou"); // Create a "ThankYou" view for the checkout confirmation
    }
    
    // Helper method to get the current client ID (implement as needed)
    private int GetCurrentClientId()
    {
        // Retrieve the client ID from the cache
        if (_cache.TryGetValue<int>("ClientId", out var clientId))
        {
            return clientId;
        }

        // If the client ID is not found in the cache, handle accordingly.
        // For example, you might redirect to a login page or return a default value.
        return 0; // Default value, update as needed        return 1; // Default value
    }
}
