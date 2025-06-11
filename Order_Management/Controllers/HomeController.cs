using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.ViewModels;
using BusinessLogicLayer.Helper;
using System.ComponentModel.DataAnnotations;

public class HomeController : Controller
{
    private readonly IOrderService _orderService;

    public HomeController(IOrderService orderService) => _orderService = orderService;

    public async Task<IActionResult> Index()
    {
        var orders = await _orderService.GetOrdersAsync();
        return View(orders);
    }

    public async Task<IActionResult> Create()
    {
        OrderViewModel model = new OrderViewModel();
        model.AvailableProducts = await _orderService.GetProductsAsync();
        ViewBag.Products = model.AvailableProducts;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(OrderViewModel model)
    {
        var success = await _orderService.CreateOrderAsync(model);
        if (success)
        {
            TempData["SuccessMessage"] = "Order created successfully!";
            return RedirectToAction("Index");
        }

        TempData["ErrorMessage"] = "Failed to create order.";
        ViewBag.Products = await _orderService.GetProductsAsync();
        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        OrderViewModel? order = await _orderService.GetOrderDetailAsync(id);
        if (order == null)
        {
            TempData["ErrorMessage"] = "Order not found.";
            return RedirectToAction("Index");
        }
        return View(order);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateOrder(List<OrderViewModel> orders)
    {
        foreach (var orderVm in orders)
        {
            await _orderService.UpdateOrderAndItemsAsync(orderVm);
        }

        TempData["SuccessMessage"] = "Orders updated successfully!";
        return RedirectToAction("Index");
    }
}