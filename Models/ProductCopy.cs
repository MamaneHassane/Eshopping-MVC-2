using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshopping_MVC.Models;

public class ProductCopy
{
    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string SerialCode{ get; set; }
    public int? ProductId { get; set; }
    public int? CartId { get; set; }
    
    public int? Quantity { get; set; }
}

/*<!--
    <a href="@Url.Action("RemoveFromCart", new { id = product.productId })" class="btn btn-danger">Remove from Cart</a>
    -->*/