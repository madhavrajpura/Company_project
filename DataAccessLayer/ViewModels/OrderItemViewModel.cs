namespace DataAccessLayer.ViewModels;

public class OrderItemViewModel
{
    public int OrderItemId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => Price * Quantity;
}