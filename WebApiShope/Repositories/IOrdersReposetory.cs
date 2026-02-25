using Entities;

namespace Repositories
{
    public interface IOrdersReposetory
    {
        Task<Order> AddOrderReposetory(Order order);
        Task<Order> GetOrderByIdReposetory(long id);
        Task<IEnumerable<OrdersItem>> GetOrderItemsReposetory(long orderId);
        Task UpdateStatusReposetory(long id ,Order order);
        Task<OrdersItem> CheckIfHasPlatformByPlatformID(long platformID);
        Task<OrdersItem?> CheckIfHasProductByProductID(long ProductsId);
        Task<IEnumerable<OrdersItem>> BringsAllPromptsReposetory(long orderId);
        Task<IEnumerable<Order>> getOrdersByUserId(long id);
        Task<IEnumerable<Order>> GetAllOrders();
    }
}