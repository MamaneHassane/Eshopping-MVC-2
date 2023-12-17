using Eshopping_MVC.Data;
using Eshopping_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eshopping_MVC.Controllers;

public class ProductsController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(AppDbContext context, ILogger<ProductsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult AddProduct()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult AddProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            //Add the game to the database 
            _context.Products.Add(product);
            _context.SaveChanges();
            // Redirect to ProductAdded with the id of the product
            return RedirectToAction("ProductAdded", new { id = product.productId});
        }
        else return View(product);
    }
    
    public IActionResult ProductAdded(int id)
    {
        // Retrive the product whit his id
        var addedProduct = _context.Products.FirstOrDefault(p => p.productId == id);
        // If null
        if (addedProduct == null) return NotFound();
        else return View(addedProduct);
    }

    public IActionResult ProductsList()
    {
        var products = _context.Products.ToList();
        // Utilisation du logger
        _logger.LogInformation("Listed all products");
        return View(products);
    }

    [HttpGet]
    public IActionResult EditProduct(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    [HttpPost]
    public IActionResult EditProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Update(product);
            _context.SaveChanges();
            return RedirectToAction("ProductAdded", new { id = product.productId });
        }
        else return View(product);
    }

    public IActionResult AddProductItem()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddProductCopy(ProductCopy productCopy)
    {
        if(ModelState.IsValid)
        {
            var associatedProduct = _context.Products.FirstOrDefault(p => p.productId == productCopy.ProductId);
            if (associatedProduct != null)
            {
                associatedProduct.ProductCopies.Add(productCopy);
                _context.SaveChanges();
                return RedirectToAction("ProductCopiesList");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Associated product not found");
            }
        }
        return View(productCopy);
    }

    public IActionResult ProductCopiesList()
    {
        var productCopies = _context.ProductCopies.ToList();
        return View(productCopies);
    }
}
