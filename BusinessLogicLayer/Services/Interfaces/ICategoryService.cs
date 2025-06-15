using DataAccessLayer.ViewModels;

namespace BusinessLogicLayer.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryViewModel>> GetAll();
    Task<CategoryViewModel?> GetById(int categoryId);
    Task<bool> Save(CategoryViewModel categoryVM);
    Task<bool> Delete(int categoryId);
    Task<bool> IsCategoryExists(CategoryViewModel categoryVM);
}