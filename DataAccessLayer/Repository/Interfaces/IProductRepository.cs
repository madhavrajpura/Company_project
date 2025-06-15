using DataAccessLayer.ViewModels;

namespace DataAccessLayer.Repository.Interfaces;

public interface IProductRepository
{
    IQueryable<ProductViewModel> GetProductsByCategoryId(int categoryid);
    Task<ProductViewModel?> GetProductById(int productId);
    Task<bool> Save(ProductViewModel productVM);
    Task<bool> Delete(int productId);
    Task<bool> IsProductExists(ProductViewModel productVM);
}