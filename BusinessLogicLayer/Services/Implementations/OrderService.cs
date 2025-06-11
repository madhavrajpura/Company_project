using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Repository.Interfaces;
using DataAccessLayer.ViewModels;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.Implementations;

public class OrderService : IOrderService
{

    private readonly IOrderRepository _orderRepo;

    public OrderService(IOrderRepository orderRepo) => _orderRepo = orderRepo;

    public async Task<List<ProductViewModel>> GetProductsAsync()
    {
        var products = await _orderRepo.GetAllProductsAsync();

        var productViewModels = products.Select(p => new ProductViewModel
        {
            ProductId = p.Id,
            ProductName = p.Name,
            Price = p.Price
        }).ToList();

        return productViewModels;
    }


    public async Task<bool> CreateOrderAsync(OrderViewModel model)
    {
        var order = new Order
        {
            OrderStatus = "In Process",
            OrderItems = model.OrderItems.Select(x => new OrderItem
            {
                ProductName = x.ProductName,
                Price = x.Price,
                Quantity = x.Quantity
            }).ToList(),
            OrderTotal = model.OrderItems.Sum(x => x.Price * x.Quantity)
        };

        return await _orderRepo.CreateOrderAsync(order);
    }


    public async Task<List<OrderViewModel>> GetOrdersAsync()
    {
        var orders = await _orderRepo.GetAllOrdersAsync();

        var data = orders.Select(o => new OrderViewModel
        {
            OrderId = o.Id,
            OrderTotal = o.OrderTotal,
            OrderStatus = o.OrderStatus,
            OrderItems = null,
        }).ToList();

        return data;
    }


    public async Task<OrderViewModel?> GetOrderDetailAsync(int id)
    {
        var order = await _orderRepo.GetOrderDetailsAsync(id);
        
        if (order == null) return null;

        var data = new OrderViewModel
        {
            OrderId = order.Id,
            OrderStatus = order.OrderStatus,
            OrderTotal = order.OrderTotal,
            OrderItems = order.OrderItems.Select(i => new OrderItemViewModel
            {
                OrderItemId = i.Id,
                ProductName = i.ProductName,
                Price = i.Price,
                Quantity = i.Quantity
            }).ToList()
        };

        return data;

    }

    public async Task<bool> ChangeOrderStatusAsync(int id, string status) => await _orderRepo.UpdateOrderStatusAsync(id, status);

    public async Task<bool> UpdateOrderAndItemsAsync(OrderViewModel vm)
    {
        var order = await _orderRepo.GetOrderDetailsAsync(vm.OrderId);

        if (order == null) return false;

        order.OrderStatus = vm.OrderStatus;

        return await _orderRepo.SaveChangesAsync();
    }
}