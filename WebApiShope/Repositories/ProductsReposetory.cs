using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ProductsReposetory : IProductsReposetory
    {
        MyShop330683525Context _DBContext;
        public ProductsReposetory(MyShop330683525Context _DBContext)
        {
            this._DBContext = _DBContext;
        }

        async public Task<(IEnumerable<Product>items,int totalCount)> GetProductsReposetory(int categoryID, int numOfPages, int PageSize, string? search,int? minPrice ,int? MaxPrice, bool? orderByPrice, bool? desc)
        {
           var quary= _DBContext.Products.Where(product =>
                (product.CategoryId == categoryID) &&
                ((search == null) ? (true) : (product.ProductsName.Contains(search))) &&
                ((minPrice == null) ? (true) : (product.Price > minPrice)) &&
                ((MaxPrice == null) ? (true) : (product.Price < MaxPrice)));

            if(orderByPrice!=null&& desc!=null&& orderByPrice != false && desc != false)
            {
                 quary = quary.OrderByDescending(x=>x.Price);

            }
            if (orderByPrice != null && (desc == null||desc==false)&& orderByPrice != false)
            {
                quary  = quary.OrderBy(x => x.Price);
            }
            else
            {
                quary = quary.OrderBy(x => x.ProductsName);     
            }
           int total=await quary.CountAsync();
          
            List<Product> items=await quary.Skip((numOfPages-1)* PageSize).Take(PageSize).
                Include(x => x.Category).ToListAsync();
            return (items, total);
        }

        async public Task<Product?>  HasProductsToCatrgoryReposetory(int categoryID)
        {
            return await _DBContext.Products.FirstOrDefaultAsync(x => x.CategoryId == categoryID);
        }

        async public Task<Product> AddProductsReposetory(Product product)
        {
            await _DBContext.Products.AddAsync(product);
            await _DBContext.SaveChangesAsync();
            return product;
        }

        async public Task UpdateProductsReposetory(int id, Product product)
        {
            _DBContext.Products.Update(product);
            await _DBContext.SaveChangesAsync();
        }

        async public Task<Product?> GetByIDProductsReposetory(int id)
        {
           return await _DBContext.Products.FirstOrDefaultAsync(x=>x.ProductsId == id);
        }


        async public Task DeleteProductsReposetory(int id)
        {
            Product productObjectToDelete = await _DBContext.Products.FirstOrDefaultAsync(x => x.ProductsId == id);

            _DBContext.Products.Remove(productObjectToDelete);
            await _DBContext.SaveChangesAsync();
           
        }




    }
}
