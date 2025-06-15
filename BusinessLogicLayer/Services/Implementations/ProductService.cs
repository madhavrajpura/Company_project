using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Repository.Interfaces;
using DataAccessLayer.ViewModels;

namespace BusinessLogicLayer.Services.Implementations;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    public ProductService(IProductRepository productRepository) => _productRepository = productRepository;

    public async Task<PaginationViewModel<ProductViewModel>> GetPaginatedProducts(int categoryId, string search, int pageNumber, int pageSize)
    {
        var query = _productRepository.GetProductsByCategoryId(categoryId);

        if (!string.IsNullOrEmpty(search))
        {
            string lowerSearchTerm = search.ToLower();
            query = query.Where(x => x.ProductName.ToLower().Contains(lowerSearchTerm) || x.Price.ToString().Contains(lowerSearchTerm)
            );
        }

        var totalCount = query.Count();

        List<ProductViewModel>? items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new PaginationViewModel<ProductViewModel>(items, totalCount, pageNumber, pageSize);
    }

    public async Task<ProductViewModel?> GetProductById(int productId)
    {
        return await _productRepository.GetProductById(productId);
    }

    public async Task<bool> Save(ProductViewModel productVM)
    {
        return await _productRepository.Save(productVM);
    }

    public async Task<bool> Delete(int productId)
    {
        return await _productRepository.Delete(productId);
    }

    public async Task<bool> IsProductExists(ProductViewModel productVM)
    {
        return await _productRepository.IsProductExists(productVM);
    }

}
