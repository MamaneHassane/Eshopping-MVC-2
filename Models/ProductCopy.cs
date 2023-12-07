using System.ComponentModel.DataAnnotations;

namespace Eshopping_MVC.Models;

public class ProductCopy
{
    [Key] 
    public string SerialNumber { get; set; }
    public int? ProductId { get; set; }
    public int? CartId { get; set; }
}