using BusinessLogicLayer.Helper;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Commerce_Application.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;


    public HomeController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index(int? categoryid, string search = "", int pageNumber = 1, int pageSize = 5)
    {
        // MainViewModel MainVM = new MainViewModel();
        // MainVM.categoryListVM = await _categoryService.GetAll();
        // ViewBag.categoryList = new SelectList(MainVM.categoryListVM, "CategoryId", "CategoryName");

        // if (categoryid == null)
        // {
        //     MainVM.productListVM = await _productService.GetPaginatedProducts(MainVM.categoryListVM[0].CategoryId, search, pageNumber, pageSize);
        // }

        return View();
    }

    public async Task<IActionResult> GetAllCategories()
    {
        MainViewModel MenuVM = new();
        MenuVM.categoryListVM = await _categoryService.GetAll();
        return PartialView("_CategoryListPartial", MenuVM);
    }

    public async Task<IActionResult> PaginatedData(int categoryid, string search = "", int pageNumber = 1, int pageSize = 5)
    {
        MainViewModel MainVM = new MainViewModel();
        MainVM.categoryListVM = await _categoryService.GetAll();

        if (categoryid > 0)
        {
            MainVM.productListVM = await _productService.GetPaginatedProducts(categoryid, search, pageNumber, pageSize);
        }

        return PartialView("_ProductListPartial", MainVM.productListVM);
    }

    public async Task<IActionResult> SaveCategory(int categoryId)
    {
        MainViewModel MainVM = new MainViewModel();
        MainVM.categoryVM = new CategoryViewModel();

        if (categoryId != 0)
        {
            MainVM.categoryVM = await _categoryService.GetById(categoryId) ?? new CategoryViewModel();
        }

        return PartialView("_SaveCategoryPartial", MainVM);
    }

    [HttpPost]
    public async Task<IActionResult> SaveCategory(MainViewModel MainVM)
    {
        bool isCategoryExists = await _categoryService.IsCategoryExists(MainVM.categoryVM);

        if (isCategoryExists)
        {
            return Json(new
            {
                success = false,
                text = NotificationMessage.EntityAlreadyExists.Replace("{0}", "Category")
            });
        }

        bool IsCategorySaved = await _categoryService.Save(MainVM.categoryVM);
        return Json(IsCategorySaved
            ? new
            {
                success = true,
                text = MainVM.categoryVM.CategoryId == 0 ?
            NotificationMessage.EntityAdd.Replace("{0}", "Category") : NotificationMessage.EntityUpdated.Replace("{0}", "Category")
            }
            : new
            {
                success = false,
                text = MainVM.categoryVM.CategoryId == 0 ?
            NotificationMessage.EntityAddFailed.Replace("{0}", "Category") : NotificationMessage.EntityUpdateFailed.Replace("{0}", "Category")
            });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteCategory(int categoryId)
    {
        if (categoryId <= 0)
        {
            return Json(new { success = false, text = NotificationMessage.EntityDoesNotExists.Replace("{0}", "Category") });
        }

        if (await _categoryService.Delete(categoryId))
        {
            return Json(new { success = true, text = NotificationMessage.EntityDeleted.Replace("{0}", "Category") });
        }
        return Json(new { success = false, text = NotificationMessage.EntityDeletionFailed.Replace("{0}", "Category") });
    }

    public async Task<IActionResult> SaveProduct(int productId)
    {
        MainViewModel MainVM = new MainViewModel();
        MainVM.productVM = new ProductViewModel();
        MainVM.categoryListVM = await _categoryService.GetAll();
        ViewBag.categoryList = new SelectList(MainVM.categoryListVM, "CategoryId", "CategoryName");

        if (productId != 0)
        {
            MainVM.productVM = await _productService.GetProductById(productId) ?? new ProductViewModel();
        }

        return PartialView("_SaveProductPartial", MainVM);
    }

[HttpPost]
    public async Task<IActionResult> SaveProduct(MainViewModel MainMV)
    {
        bool isProductExists = await _productService.IsProductExists(MainMV.productVM);

        if (isProductExists)
        {
            return Json(new
            {
                success = false,
                text = NotificationMessage.EntityAlreadyExists.Replace("{0}", "Product")
            });
        }

        bool IsProductSaved = await _productService.Save(MainMV.productVM);
        return Json(IsProductSaved
            ? new
            {
                success = true,
                text = MainMV.productVM.ProductId == 0 ?
            NotificationMessage.EntityAdd.Replace("{0}", "Product") : NotificationMessage.EntityUpdated.Replace("{0}", "Product")
            }
            : new
            {
                success = false,
                text = MainMV.productVM.ProductId == 0 ?
            NotificationMessage.EntityAddFailed.Replace("{0}", "Product") : NotificationMessage.EntityUpdateFailed.Replace("{0}", "Product")
            });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteProduct(int productId)
    {
        if (productId <= 0)
        {
            return Json(new { success = false, text = NotificationMessage.EntityDoesNotExists.Replace("{0}", "Product") });
        }

        if (await _productService.Delete(productId))
        {
            return Json(new { success = true, text = NotificationMessage.EntityDeleted.Replace("{0}", "Product") });
        }
        return Json(new { success = false, text = NotificationMessage.EntityDeletionFailed.Replace("{0}", "Product") });
    }

}
