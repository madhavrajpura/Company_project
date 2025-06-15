using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Repository.Interfaces;
using DataAccessLayer.ViewModels;

namespace BusinessLogicLayer.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    public CategoryService(ICategoryRepository categoryRepository) => _categoryRepository = categoryRepository;

    public async Task<List<CategoryViewModel>> GetAll() => await _categoryRepository.GetAll();
    
    public async Task<CategoryViewModel?> GetById(int categoryId) => await _categoryRepository.GetById(categoryId);

    public async Task<bool> Save(CategoryViewModel categoryVM) => await _categoryRepository.Save(categoryVM);

    public async Task<bool> Delete(int categoryId) => await _categoryRepository.Delete(categoryId);

    public async Task<bool> IsCategoryExists(CategoryViewModel categoryVM) => await _categoryRepository.IsCategoryExists(categoryVM);

}