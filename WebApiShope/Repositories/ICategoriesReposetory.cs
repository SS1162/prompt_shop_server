using Entities;

namespace Repositories
{
    public interface ICategoriesReposetory
    {
        Task<Category> AddCategoriesReposetory(Category categoryToUpdate);
        Task DeleteIDCategoriesReposetory(int id);
        Task<Category?> GetByIDCategoriesReposetory(long id);
        Task<(IEnumerable<Category> items, int totalCount)> GetCategoriesReposetory(int numberOfPages, int mainCategoryID, int pageSize, string? search);
        Task UpdateCategoriesReposetory(int id, Category categoryToUpdate);
        Task<Category?> GetByMainCategoriesIDReposetory(int id);
    }
}