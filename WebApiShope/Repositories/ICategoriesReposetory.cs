using Entities;

namespace Repositories
{
    public interface ICategoriesReposetory
    {
        Task<Category> AddCategoriesReposetory(Category categoryToUpdate);
        Task DeleteIDCategoriesReposetory(long id);
        Task<Category?> GetByIDCategoriesReposetory(long id);
        Task<(IEnumerable<Category> items, int totalCount)> GetCategoriesReposetory(int numberOfPages, long mainCategoryID, int pageSize, string? search);
        Task UpdateCategoriesReposetory(long id, Category categoryToUpdate);
        Task<Category?> GetByMainCategoriesIDReposetory(long id);
    }
}