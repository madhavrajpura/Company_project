using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;
using DataAccessLayer.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository.Implementations;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDBContext _db;
    public ProductRepository(ApplicationDBContext db) => _db = db;

    public IQueryable<ProductViewModel> GetProductsByCategoryId(int categoryid)
    {
        IQueryable<ProductViewModel> data = _db.Products
           .Include(p => p.Category)
           .Where(pc => pc.CategoryId == categoryid).OrderBy(pc => pc.Id)
           .Select(x => new ProductViewModel
           {
               ProductId = x.Id,
               ProductName = x.Name,
               Details = x.Details,
               Price = x.Price,
               ImageUrl = x.ImageUrl,
               CategoryId = x.CategoryId,
               CategoryName = x.Category.Name,
               IsActive = x.IsActive
           }).AsQueryable();

        return data;
    }

    public async Task<ProductViewModel?> GetProductById(int productId)
    {
        return await _db.Products
            .Include(p => p.Category)
            .Where(p => p.Id == productId)
            .Select(p => new ProductViewModel
            {
                ProductId = p.Id,
                ProductName = p.Name,
                Details = p.Details,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                IsActive = p.IsActive
            })
            .FirstOrDefaultAsync() ?? new ProductViewModel();
    }

    public async Task<bool> Save(ProductViewModel productVM)
    {
        if (productVM == null) return false;

        if (productVM.ProductId == 0)
        {
            Product newProduct = new Product
            {
                Name = productVM.ProductName,
                Details = productVM.Details,
                Price = productVM.Price,
                ImageUrl = productVM.ImageUrl,
                CategoryId = productVM.CategoryId,
                IsActive = productVM.IsActive,
                CreatedAt = DateTime.Now
            };
            _db.Products.Add(newProduct);
        }
        else
        {
            Product? existingProduct = await _db.Products.FirstOrDefaultAsync(p => p.Id == productVM.ProductId);

            if (existingProduct == null) return false;

            existingProduct.Name = productVM.ProductName;
            existingProduct.Details = productVM.Details;
            existingProduct.Price = productVM.Price;
            existingProduct.ImageUrl = productVM.ImageUrl;
            existingProduct.CategoryId = productVM.CategoryId;
            existingProduct.IsActive = productVM.IsActive;
            existingProduct.UpdatedAt = DateTime.Now;
            _db.Products.Update(existingProduct);
        }

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(int productId)
    {
        Product? product = await _db.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (product == null) return false;

        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsProductExists(ProductViewModel productVM)
    {
        if (productVM == null) return false;

        if (productVM.ProductId == 0)
        {
            return await _db.Products.AnyAsync(x => x.Name.ToLower().Trim() == productVM.ProductName.ToLower().Trim() && x.CategoryId == productVM.CategoryId);
        }
        else
        {
            return await _db.Products.AnyAsync(x => x.Id != productVM.ProductId && x.Name.ToLower().Trim() == productVM.ProductName.ToLower().Trim() && x.CategoryId == productVM.CategoryId);
        }
    }
}