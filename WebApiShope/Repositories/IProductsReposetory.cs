using Entities;

namespace Repositories
{
    public interface IProductsReposetory
    {
        Task<Product> AddProductsReposetory(Product product);
        Task DeleteProductsReposetory(long id);
        Task<(IEnumerable<Product> items, int totalCount)> GetProductsReposetory(long categoryID, int numOfPages, int PageSize, string? search, int? minPrice, int? MaxPrice, bool? orderByPrice, bool? desc);
        Task UpdateProductsReposetory(long id, Product product);
        Task<Product?> GetByIDProductsReposetory(long id);
        Task<Product?> HasProductsToCatrgoryReposetory(long categoryID);
    }
}