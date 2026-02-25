using DTO;

namespace Services
{
    public interface IOrdersServise
    {
        Task<Resulte<FullOrderDTO>> AddOrderServise(OrdersDTO dto);
        Task<OrderDetielsDTO> GetByIdOrderServise(long id);
        Task<Resulte<IEnumerable<OrderItemDTO>>> GetOrderItemsServise(long orderId);
        Task<Resulte<FullOrderDTO>> UpdateStatusServise(long id, FullOrderDTO order);
        Task<IEnumerable<FullOrderDTO>> GetAllOrders();
        Task<IEnumerable<FullOrderDTO>> GetByUserIdOrderServise(long id);
    }
}