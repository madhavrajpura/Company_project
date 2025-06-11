using DataAccessLayer.Models;

namespace DataAccessLayer.Repository.Interfaces;

public interface IOrderRepository
{


    Task<List<Product>> GetAllProductsAsync();
    Task<bool> CreateOrderAsync(Order order);
    Task<List<Order>> GetAllOrdersAsync();
    Task<Order?> GetOrderDetailsAsync(int orderId);
    Task<bool> UpdateOrderStatusAsync(int orderId, string status);
    Task<bool> SaveChangesAsync();

}
