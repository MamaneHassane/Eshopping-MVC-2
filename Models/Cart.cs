using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshopping_MVC.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        public ICollection<Product>? Products { get; set; } = new List<Product>();
        
        public int? ClientId { get; set; }
        //public string CartProductIds { get; set; }  
        [ForeignKey("ClientId")]
        public Client Client { get; set; }
        public void AddProductById(int productId)
        {
            
        }
        public void ClearCart()
        {
            //CartProductIds = "[]";
            Products = new List<Product>();
        }
    }
}