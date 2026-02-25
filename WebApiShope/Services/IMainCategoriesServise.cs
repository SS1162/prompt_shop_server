using DTO;

namespace Services
{
    public interface IMainCategoriesServise
    {
        Task<MainCategoriesDTO> AddMainCategoriesServises(ManegerMainCategoryDTO manegerMainCategory);
        Task<Resulte<MainCategoriesDTO>> DeleteMainCategoriesServises(long id);
        Task<IEnumerable<MainCategoriesDTO>> GetMainCategoriesServises();
        Task<Resulte<MainCategoriesDTO>> UpdateMainCategoriesServises(long id, MainCategoriesDTO MainCategoriesFromController);
    }
}