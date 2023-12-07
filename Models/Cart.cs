using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Eshopping.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        public ICollection<Product>? Products { get; set; } = new List<Product>();
        
        public int? ClientId { get; set; }
        public string CartProductIds { get; set; }  

        public void AddProductById(int productId)
        {
            
        }
        public void ClearCart()
        {
            CartProductIds = "[]";
            Products = new List<Product>();
        }
    }
}