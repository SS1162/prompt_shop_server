using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;


namespace Repositories
{
    public class CartsReposetory : ICartsReposetory
    {

        private readonly MyShop330683525Context _DBcontext;


      
        public CartsReposetory(MyShop330683525Context _DBcontext)
        {
            this._DBcontext = _DBcontext;
        }

        public async Task<CartItem?> GetByIdReposetory(long id)
        {
            return await _DBcontext.CartItems.FirstOrDefaultAsync(c => c.CartId == id);
        }
        public async Task<IEnumerable<CartItem>> GetByIDUserCartItemsReposetory(long Id)
        {
            return await _DBcontext.CartItems
                .Where(ci => ci.UserId == Id)
                .ToListAsync();
        }

        public async Task<CartItem> CreateUserCartReposetory(CartItem cartItem)
        {
            await _DBcontext.CartItems.AddAsync(cartItem);
            await _DBcontext.SaveChangesAsync();
            return cartItem;
        }

        

        public async Task ChangeProductToValidReposetory(long Id)
        {
            CartItem cartItem = await _DBcontext.CartItems.FirstOrDefaultAsync(x=>x.CartId==Id);
            cartItem.Valid = 1;
            _DBcontext.CartItems.Update(cartItem);
            await _DBcontext.SaveChangesAsync();

        }


        public async Task<CartItem?> CheckIfHasPlatformByPlatformID(long Id)
        {
           return await _DBcontext.CartItems.FirstOrDefaultAsync(x=>x.BasicSitesPlatforms==Id);

        }


        public async Task<CartItem?> CheckIfHasProductByProductID(long Id)
        {
            return await _DBcontext.CartItems.FirstOrDefaultAsync(x => x.ProductsId == Id);

        }
        public async Task ChangeProductToNotValidReposetory(long Id)
        {
            CartItem cartItem = await _DBcontext.CartItems.FirstOrDefaultAsync(x => x.CartId == Id);
            cartItem.Valid = 0;
            _DBcontext.CartItems.Update(cartItem);
            await _DBcontext.SaveChangesAsync();

        }

        //       מוחקת סל ומוסיפה הזמנות
        public async Task DeleteUserCartReposetory(long userID)
        {
            List<CartItem> itemList = await _DBcontext.CartItems.Where(x => x.UserId == userID).ToListAsync();
            for (int i = 0; i < itemList.Count(); i++)
            {
                _DBcontext.CartItems.Remove(itemList[i]);
            }
            await _DBcontext.SaveChangesAsync();
        }

        public async Task<CartItem?> GetByUserAndProductIdReposetory(long userId, long productId)
        {
            return await _DBcontext.CartItems.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductsId == productId);
        }
        public async Task DeleteUserCartItemReposetory(long Id)
        {
            var cartItemsObjectToDelete = await _DBcontext.CartItems.FirstOrDefaultAsync(x => x.CartId == Id);
            _DBcontext.CartItems.Remove(cartItemsObjectToDelete);
            await _DBcontext.SaveChangesAsync();
        }

       

       
    }
}
