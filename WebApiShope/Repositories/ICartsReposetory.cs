using Entities;

namespace Repositories
{
    public interface ICartsReposetory
    {
        Task ChangeProductToNotValidReposetory(long Id);
        Task ChangeProductToValidReposetory(long Id);
        Task<CartItem> CreateUserCartReposetory(CartItem cartItem);
        Task DeleteUserCartItemReposetory(long Id);
        Task DeleteUserCartReposetory(long userID);
        Task<CartItem?> GetByIdReposetory(long id);
        Task<IEnumerable<CartItem>> GetByIDUserCartItemsReposetory(long Id);
        Task<CartItem?> GetByUserAndProductIdReposetory(long userId, long productId);

        Task<CartItem?> CheckIfHasPlatformByPlatformID(long Id);

        Task<CartItem?> CheckIfHasProductByProductID(long Id);

    }
}