using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;

namespace BusinessLogicLayer.Services.Interfaces;

public interface IOrderService
{

    Task<List<ProductViewModel>> GetProductsAsync();
    Task<bool> CreateOrderAsync(OrderViewModel model);
    Task<List<OrderViewModel>> GetOrdersAsync();
    Task<OrderViewModel?> GetOrderDetailAsync(int id);
    Task<bool> ChangeOrderStatusAsync(int id, string status);
    Task<bool> UpdateOrderAndItemsAsync(OrderViewModel vm);
}
