using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;

namespace DataAccessLayer.Repository.Interfaces;

public interface ICategoryRepository
{
    Task<List<CategoryViewModel>> GetAll();
    Task<CategoryViewModel?> GetById(int CategoryId);
    Task<bool> Save(CategoryViewModel categoryVM);
    Task<bool> Delete(int CategoryId);
    Task<bool> IsCategoryExists(CategoryViewModel categoryVM);
}