using DTO;

namespace Services
{
    public interface ICartItemServise
    {
        Task<Resulte<CartItemDTO?>> ChangeProductToNotValidCartServise(long cartItemId);
        Task<Resulte<CartItemDTO?>> ChangeProductToValidCartServise(long cartItemId);
      



        Task<Resulte<CartItemDTO?>> DeleteUserCartServise(long cartItemId);


        Task<Resulte<CartItemDTO>> CreateUserCartServise(AddToCartDTO dto);
        Task<CartItemDTO?> GetByIdServise(long id);


        Task<Resulte<IEnumerable<CartItemDTO>>> GetUserCartServise(long userId);
    }
}