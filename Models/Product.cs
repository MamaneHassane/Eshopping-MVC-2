using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace Eshopping.Models;
using System.ComponentModel.DataAnnotations;

[Serializable]
public class Product
{
    [Key]
    [DatabaseGenerated((DatabaseGeneratedOption.Identity))]
    public int productId { get; set; }
    [Required] [DisplayName("ProductName")] 
    public string Name { get; set; }
    [Required]
    public double Price { get; set; }	
    public string Description { get; set; }
    public string ImageUrl { get; set; }  
    [Required]
    public int Quantity { get; set; }

    public ICollection<Cart>? carts { get; set; }
    
}