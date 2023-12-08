using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshopping_MVC.Models;

[Serializable]
public class Client
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int clientId { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public Cart? cart { get; set; }
}