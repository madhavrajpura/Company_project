using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models;

public class Order
{
    [Key]
    public int Id { get; set; }
    public decimal OrderTotal{get;set;}
    public string OrderStatus { get; set; } = "In Process";
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

}
