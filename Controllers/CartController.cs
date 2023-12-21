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
        var clientId = GetCurrentClientId();
        var clientWithCart = _context.Clients.Include(c => c.Cart).ThenInclude(cart => cart.Products).FirstOrDefault(c => c.clientId == clientId);

        if (clientWithCart == null)
        {
            return View("Error"); // Handle the case where the client or cart is not found
        }

        var cart = clientWithCart.Cart;

        if (cart == null || cart.Products == null || cart.Products.Count == 0)
        {
            // Le client n'a pas de panier ou le panier est vide
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
            // Si le client n'a pas de panier, créez-en un nouveau
            var newCart = new Cart();
            client.Cart = newCart;
            _context.Carts.Add(newCart);
        }

        var cart = client.Cart;

        // Vérifier si le produit est déjà dans le panier
        var existingProduct = cart.Products.FirstOrDefault(p => p.productId == productId);

        if (existingProduct != null)
        {
            // Le produit est déjà dans le panier, augmentez la quantité de 1
            existingProduct.Quantity++;
        }
        else
        {
            // Le produit n'est pas encore dans le panier, ajoutez-le avec une quantité de 1
            var product = _context.Products.Find(productId);

            if (product != null)
            {
                if (product.Quantity == 1) product.Quantity++;
                else product.Quantity = 1; // Nouveau produit avec une quantité de 1
                if(product.Quantity==1) cart.Products.Add(product);
            }
        }

        // Sauvegarder une seule fois après les opérations conditionnelles
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    // Add other actions as needed (e.g., RemoveFromCart, Checkout, etc.)
    public IActionResult RemoveFromCart(int productId)
    {
        var clientId = GetCurrentClientId();
        var cart = _context.Carts.Include(c => c.Products).FirstOrDefault(c => c.ClientId == clientId);

        if (cart == null)
        {
            return View("Error");
        }

        // Vérifier si le produit est dans le panier avant de le supprimer
        var productToRemove = cart.Products.FirstOrDefault(p => p.productId == productId);

        if (productToRemove != null)
        {
            // Créer un nouveau panier avec tous les autres produits
            var newCart = new Cart
            {
                Products = cart.Products.Where(p => p.productId != productId).ToList(),
                // Copier d'autres propriétés du panier si nécessaire
            };

            // Remplacer l'ancien panier par le nouveau panier
            cart = newCart;

            // Sauvegarder les modifications dans la base de données
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }

    
    [HttpGet]
    public IActionResult DeleteCart()
    {
        var role = User.FindFirst("Role")?.Value;

        if (role == "User")
        {
            // Si l'utilisateur est un client, supprimez le panier de la cache
            _cache.Remove("ClientId");
        }

        // Supprimer le panier de la base de données (vous devrez adapter ceci selon votre modèle de données)
        // Assurez-vous de gérer les relations entre les tables pour éviter les contraintes d'intégrité.
        var clientId = GetCurrentClientId();
        var client = _context.Clients.Include(c => c.Cart).FirstOrDefault(c => c.clientId == clientId);

        if (client != null && client.Cart != null)
        {
            _context.Clients.FirstOrDefault(c => c.clientId == client.clientId).Cart = new Cart();
            _context.SaveChanges();
        }

        // Rediriger vers la page d'accueil ou une autre page après la suppression du panier
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