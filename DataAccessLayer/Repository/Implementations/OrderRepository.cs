using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository.Implementations;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDBContext _context;

    public OrderRepository(ApplicationDBContext context) => _context = context;

    public async Task<List<Product>> GetAllProductsAsync() {
        var result = await _context.Products.ToListAsync();
        return result;
    }

    public async Task<bool> CreateOrderAsync(Order order)
    {
        _context.Orders.Add(order);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        var result = await _context.Orders.Include(o => o.OrderItems).ToListAsync();
        return result;
    }

    public async Task<Order?> GetOrderDetailsAsync(int orderId){
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == orderId);
        return order;
    }

    public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null) return false;
        order.OrderStatus = status;
        return await _context.SaveChangesAsync() > 0;
    }


    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

}