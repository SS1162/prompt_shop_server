using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CategoriesReposetory : ICategoriesReposetory
    {
        private readonly MyShop330683525Context _DBContext;
        public CategoriesReposetory(MyShop330683525Context _DBContext)
        {
            this._DBContext = _DBContext;
        }
        async public Task<(IEnumerable<Category>items,int totalCount)> GetCategoriesReposetory(int numberOfPages, long mainCategoryID, int pageSize, string? search)
        {

            var quary = _DBContext.Categories.Where(
                category =>
                (category.MainCategoryId == mainCategoryID) &&
                ((search == null) ? (true) : (category.CategoryName.Contains(search) || category.CategoryDescreption.Contains(search))))
                .OrderBy(x=>x.CategoryName);

            int totalcount = await quary.CountAsync();
            List<Category> items =await  quary.Skip((numberOfPages - 1) * pageSize).Take(pageSize).ToListAsync();

            return (items, totalcount);
        }



        async public Task<Category?> GetByMainCategoriesIDReposetory(long id)
        {
            return await _DBContext.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.MainCategoryId == id);

        }
        async public Task<Category?> GetByIDCategoriesReposetory(long id)
        {
            return await _DBContext.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.CategoryId == id);

        }

        async public Task UpdateCategoriesReposetory(long id, Category categoryToUpdate)
        {
            _DBContext.Categories.Update(categoryToUpdate);
            await _DBContext.SaveChangesAsync();

        }


        async public Task<Category> AddCategoriesReposetory(Category categoryToUpdate)
        {
            await _DBContext.Categories.AddAsync(categoryToUpdate);
            await _DBContext.SaveChangesAsync();
            return categoryToUpdate;

        }
        async public Task DeleteIDCategoriesReposetory(long id)
        {
            Category category = await _DBContext.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);
           
                _DBContext.Categories.Remove(category);
                await _DBContext.SaveChangesAsync();
        }


    }
}
