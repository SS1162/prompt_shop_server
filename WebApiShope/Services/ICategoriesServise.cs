using DTO;

namespace Services
{
    public interface ICategoriesServise
    {
        Task<Resulte<CategoryDTO>> AddCategoriesServise(AddCategoryDTO categoryToAdd);
        Task<Resulte<CategoryDTO>> DeleteIDCategoriesServise(long id);
        Task<CategoryDTO> GetByIDCategoriesServise(long id);
        Task<Resulte<ResponePage<CategoryDTO>>> GetCategoriesServise(int numberOfPages, long mainCategoryID, int pageSize, string? search);
        Task<Resulte<CategoryDTO?>> UpdateCategoriesServise(long id, CategoryToUpdateDTO categoryToUpdate);
    }
}