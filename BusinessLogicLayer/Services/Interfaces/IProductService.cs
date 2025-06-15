using DataAccessLayer.ViewModels;

namespace BusinessLogicLayer.Services.Interfaces;

public interface IProductService
{
    Task<PaginationViewModel<ProductViewModel>> GetPaginatedProducts(int categoryId, string search, int pageNumber, int pageSize);
    Task<ProductViewModel?> GetProductById(int productId);
    Task<bool> Save(ProductViewModel productVM);
    Task<bool> Delete(int productId);
    Task<bool> IsProductExists(ProductViewModel productVM);
}