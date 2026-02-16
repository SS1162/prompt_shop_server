using Entities;

namespace Repositories
{
    public interface IMainCategoriesReposetory
    {
        Task<MainCategory> AddMainCategoriesReposetoty(MainCategory mainCategoryToAdd);
        Task DeleteMainCategoriesReposetoty(long id);
        Task<IEnumerable<MainCategory>> GetMainCategoriesReposetoty();
        Task UpdateMainCategoriesReposetoty(long id, MainCategory mainCategoryToUpdate);
        Task<MainCategory?> GetByIdMainCategoriesReposetoty(long id);
    }
}