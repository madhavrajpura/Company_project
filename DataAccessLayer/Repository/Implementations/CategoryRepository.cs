using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;
using DataAccessLayer.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository.Implementations;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDBContext _db;
    public CategoryRepository(ApplicationDBContext db) => _db = db;

    public async Task<List<CategoryViewModel>> GetAll()
    {
        return await _db.Categories
            .Select(c => new CategoryViewModel
            {
                CategoryId = c.Id,
                CategoryName = c.Name,
                IsActive = c.IsActive
            })
            .ToListAsync();
    }

    public async Task<CategoryViewModel?> GetById(int CategoryId)
    {
        return await _db.Categories
            .Where(c => c.Id == CategoryId)
            .Select(c => new CategoryViewModel
            {
                CategoryId = c.Id,
                CategoryName = c.Name,
                IsActive = c.IsActive
            })
            .FirstOrDefaultAsync() ?? new CategoryViewModel();
    }

    public async Task<bool> Save(CategoryViewModel categoryVM)
    {
        if (categoryVM == null) return false;

        if (categoryVM.CategoryId == 0)
        {
            Category Addcategory = new Category
            {
                Name = categoryVM.CategoryName,
                IsActive = categoryVM.IsActive,
                CreatedAt = DateTime.Now
            };
            _db.Categories.Add(Addcategory);
        }
        else
        {
            Category? existingCategory = await _db.Categories.FirstOrDefaultAsync(c => c.Id == categoryVM.CategoryId);

            if (existingCategory == null) return false;

            existingCategory.Name = categoryVM.CategoryName;
            existingCategory.IsActive = categoryVM.IsActive;
            existingCategory.UpdatedAt = DateTime.Now;
            _db.Categories.Update(existingCategory);
        }

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(int CategoryId)
    {
        Category? category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == CategoryId);

        if (category == null) return false;

        _db.Categories.Remove(category);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsCategoryExists(CategoryViewModel categoryVM)
    {
        if (categoryVM == null) return false;

        if (categoryVM.CategoryId == 0) return await _db.Categories.AnyAsync(c => c.Name.ToLower().Trim() == categoryVM.CategoryName.ToLower().Trim());

        return await _db.Categories.AnyAsync(c => c.Name.ToLower().Trim() == categoryVM.CategoryName.ToLower().Trim() && c.Id != categoryVM.CategoryId);
    }

}