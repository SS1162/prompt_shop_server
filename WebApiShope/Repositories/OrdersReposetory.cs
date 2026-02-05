using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;
namespace Repositories
{
    public class OrdersReposetory : IOrdersReposetory
    {

        MyShop330683525Context _DBcontext;
        ICartsReposetory _cartsReposetory;

        public OrdersReposetory(MyShop330683525Context _DBcontext, ICartsReposetory cartsReposetory)
        {
            this._DBcontext = _DBcontext;
            this._cartsReposetory = cartsReposetory;
        }

        public OrdersReposetory(MyShop330683525Context _DBcontext)
        {
            this._DBcontext = _DBcontext;
       
        }

        public async Task<Order?> GetOrderByIdReposetory(int id)
        {
            return await _DBcontext.Orders.FirstOrDefaultAsync(order => order.OrderId == id);
        }

        public async Task<Order> AddOrderReposetory(Order order)
        {
            await _DBcontext.Orders.AddAsync(order);
            await _DBcontext.SaveChangesAsync();
            await _cartsReposetory.DeleteUserCartReposetory(Convert.ToInt32(order.UserId));
            return order;
        }

        public async Task UpdateStatusReposetory(int id ,Order order)
        {
            _DBcontext.Orders.Update(order);
            await _DBcontext.SaveChangesAsync();
        }



        public async Task<IEnumerable<OrdersItem>> GetOrderItemsReposetory(int orderId)
        {
            return await _DBcontext.OrdersItems
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<OrdersItem?> CheckIfHasPlatformByPlatformID(int platformID)
        {
            return await _DBcontext.OrdersItems.FirstOrDefaultAsync(x => x.BasicSitesPlatforms == platformID);
        }

        public async Task<OrdersItem?> CheckIfHasProductByProductID(int ProductsId)
        {
            return await _DBcontext.OrdersItems.FirstOrDefaultAsync(x => x.ProductsId == ProductsId);
        }


    }

}

