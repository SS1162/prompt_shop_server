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

        private readonly MyShop330683525Context _DBcontext;
        private readonly ICartsReposetory _cartsReposetory;

        public OrdersReposetory(MyShop330683525Context _DBcontext, ICartsReposetory cartsReposetory)
        {
            this._DBcontext = _DBcontext;
            this._cartsReposetory = cartsReposetory;
        }

        public OrdersReposetory(MyShop330683525Context _DBcontext)
        {
            this._DBcontext = _DBcontext;
       
        }

        public async Task<Order?> GetOrderByIdReposetory(long id)
        {
            return await _DBcontext.Orders.FirstOrDefaultAsync(order => order.OrderId == id);
        }

        public async Task<Order> AddOrderReposetory(Order order)
        {
            await _DBcontext.Orders.AddAsync(order);
            await _DBcontext.SaveChangesAsync();
            await _cartsReposetory.DeleteUserCartReposetory(order.UserId);
            return order;
        }

        public async Task UpdateStatusReposetory(long id ,Order order)
        {
            _DBcontext.Orders.Update(order);
            await _DBcontext.SaveChangesAsync();
        }



        public async Task<IEnumerable<OrdersItem>> GetOrderItemsReposetory(long orderId)
        {
            return await _DBcontext.OrdersItems
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<OrdersItem?> CheckIfHasPlatformByPlatformID(long platformID)
        {
            return await _DBcontext.OrdersItems.FirstOrDefaultAsync(x => x.BasicSitesPlatforms == platformID);
        }

        public async Task<OrdersItem?> CheckIfHasProductByProductID(long productsId)
        {
            return await _DBcontext.OrdersItems.FirstOrDefaultAsync(x => x.ProductsId == productsId);
        }

        public async Task<IEnumerable<OrdersItem>> BringsAllPromptsReposetory(long orderId)
        {
            return await _DBcontext.OrdersItems
                 .Where(x => x.OrderId == orderId)
               .Select(oi => new OrdersItem
                {
            OrderItemId = oi.OrderItemId,
         ProductsId = oi.ProductsId,
           OrderId = oi.OrderId,
     UserDescription = oi.UserDescription,
         BasicSitesPlatforms = oi.BasicSitesPlatforms,
       Products = new Product
          {
         ProductsId = oi.Products.ProductsId,
        ProductPrompt = oi.Products.ProductPrompt,
     Category = new Category
       {
          CategoryId = oi.Products.Category.CategoryId,
                CategoryPrompt = oi.Products.Category.CategoryPrompt,
      MainCategory = new MainCategory
            {
               MainCategoryId = oi.Products.Category.MainCategory.MainCategoryId,
          MainCategoryPrompt = oi.Products.Category.MainCategory.MainCategoryPrompt
    }
           }
          }
              })
                    .OrderBy(x => x.Products.Category.MainCategory.MainCategoryId)
                .ThenBy(x => x.Products.Category.CategoryId)
          .ToListAsync();
        }
    }

}

