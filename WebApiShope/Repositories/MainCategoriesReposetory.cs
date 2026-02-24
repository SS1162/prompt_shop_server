using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;
namespace Repositories
{
    public class MainCategoriesReposetory : IMainCategoriesReposetory
    {
        private readonly MyShop330683525Context _DBContext;
        public MainCategoriesReposetory(MyShop330683525Context _DBContext)
        {
            this._DBContext = _DBContext;
        }
        async public Task<IEnumerable<MainCategory>> GetMainCategoriesReposetoty()
        {
            return await _DBContext.MainCategories.ToListAsync();
        }


        async public Task<MainCategory> AddMainCategoriesReposetoty(MainCategory mainCategoryToAdd)
        {

            await _DBContext.MainCategories.AddAsync(mainCategoryToAdd);
            await _DBContext.SaveChangesAsync();
            return mainCategoryToAdd;
        }




        async public Task UpdateMainCategoriesReposetoty(long id, MainCategory mainCategoryToUpdate)
        {

            _DBContext.MainCategories.Update(mainCategoryToUpdate);
            await _DBContext.SaveChangesAsync();

        }


        async public Task<MainCategory?> GetByIdMainCategoriesReposetoty(long id)
        {
            return await _DBContext.MainCategories.AsNoTracking().FirstOrDefaultAsync(x => x.MainCategoryId == id);
        }



        //עדיין אין בדיקות
        async public Task DeleteMainCategoriesReposetoty(long id)
        {
            MainCategory mainCategory = await _DBContext.MainCategories.FirstOrDefaultAsync(x=>x.MainCategoryId==id);
            _DBContext.Remove(mainCategory);
            await _DBContext.SaveChangesAsync();
        }




    }
}
