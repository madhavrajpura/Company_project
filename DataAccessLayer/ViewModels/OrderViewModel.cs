namespace DataAccessLayer.ViewModels;

public class OrderViewModel
{
    public int OrderId { get; set; }    
    public decimal OrderTotal { get; set; }
    public string OrderStatus { get; set; } = "In Process";
    public List<OrderItemViewModel> OrderItems { get; set; }
    public List<ProductViewModel> AvailableProducts{get; set;}
}