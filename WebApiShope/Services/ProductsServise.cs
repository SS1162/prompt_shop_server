using AutoMapper;
using DTO;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Services
{
    public class ProductsServise : IProductsServise
    {

        private readonly IProductsReposetory _productsReposetory;
        private readonly IMapper _mapper;
        private readonly ICategoriesReposetory _categoriesReposetory;
        private readonly ICartsReposetory _cartReposetory;
        private readonly IOrdersReposetory _ordersReposetory;
        public ProductsServise(IProductsReposetory productsReposetory,
        IMapper mapper, ICategoriesReposetory categoriesReposetory, ICartsReposetory cartReposetory, IOrdersReposetory ordersReposetory)
        {
            this._productsReposetory = productsReposetory;
            this._mapper = mapper;
            this._categoriesReposetory = categoriesReposetory;
            _cartReposetory = cartReposetory;
            _ordersReposetory = ordersReposetory;
        }

        async public Task<Resulte<ResponePage<ProductDTO>>> GetProductsServise(long categoryID, int numOfPages, int PageSize, string? search, int? minPrice, int? MaxPrice, bool? orderByPrice, bool? desc)
        {


            if (orderByPrice == null && desc != null)
            {
                return Resulte<ResponePage<ProductDTO>>.Failure("You can't add desc without orderBy");
            }
            if (minPrice > MaxPrice)
            {
                return Resulte<ResponePage<ProductDTO>>.Failure("Min price is bigger then max price");
            }

            if (PageSize > 50)
            {
                return Resulte<ResponePage<ProductDTO>>.Failure("Page size is too big");
            }

            Category? toCheckIfCategotyInsist = await _categoriesReposetory.GetByIDCategoriesReposetory(categoryID);
            if (toCheckIfCategotyInsist == null)
            {
                return Resulte<ResponePage<ProductDTO>>.Failure("Category ID is not insist");
            }

            (IEnumerable<Product>, int) responeFromReposetory = await _productsReposetory.GetProductsReposetory(categoryID, numOfPages, PageSize, search, minPrice, MaxPrice, orderByPrice, desc);

            ResponePage<ProductDTO> responeForClient = new ResponePage<ProductDTO>();

            responeForClient.CurrentPage = numOfPages;
            responeForClient.PageSize = PageSize;

            responeForClient.HasPreviousPage = numOfPages > 0;
            responeForClient.HasNextPage = (numOfPages - 1) * PageSize < responeFromReposetory.Item2;

            responeForClient.TotalItems = responeFromReposetory.Item2;
            responeForClient.Data = _mapper.Map<IEnumerable<ProductDTO>>(responeFromReposetory.Item1);


            return Resulte<ResponePage<ProductDTO>>.Success(responeForClient);
        }

        async public Task<Resulte<ProductDTO>> UpdateProductServise(long id, UpdateProductDTO productToUpdate)
        {
            if (id != productToUpdate.ProductID)
            {
                return Resulte<ProductDTO>.Failure("The id's are diffrent");
            }
            Category? checkIfCategoryExist = await _categoriesReposetory.GetByIDCategoriesReposetory(productToUpdate.CategoryID);
            if (checkIfCategoryExist == null)
            {
                return Resulte<ProductDTO>.Failure("The category id is'nt exist");
            }

            Product productToReposetory = _mapper.Map<Product>(productToUpdate);
            //למלא פרומפט עם gemini
            productToReposetory.ProductPrompt = "vfsghhfg";
            await _productsReposetory.UpdateProductsReposetory(id, productToReposetory);
            return Resulte<ProductDTO>.Success(null);

        }


        async public Task<Resulte<ProductDTO>> AddProductServise(AddProductDTO productToUpdate)
        {
            Category? checkIfCategoryExist = await _categoriesReposetory.GetByIDCategoriesReposetory(productToUpdate.CategoryID);
            if (checkIfCategoryExist == null)
            {
                return Resulte<ProductDTO>.Failure("The category id is'nt exist");
            }
            Product productToReposetory = _mapper.Map<Product>(productToUpdate);
            //למלא פרומפט עם gemini
            productToReposetory.ProductPrompt = "gfasdfghfh";
            Product productFromReposetory = await _productsReposetory.AddProductsReposetory(productToReposetory);

            return Resulte<ProductDTO>.Success(_mapper.Map<ProductDTO>(productFromReposetory));
        }
        async public Task<Resulte<ProductDTO>> DeleteIDProductServise(long id)
        {


            var checkIfCartItemExist = await _cartReposetory.CheckIfHasProductByProductID(id);
            if (checkIfCartItemExist != null)
            {
                return Resulte<ProductDTO>.Failure("There is a cart items that referenc to product");
            }


            var checkIfOredersItemExist = await _ordersReposetory.CheckIfHasProductByProductID(id);
            if (checkIfOredersItemExist != null)
            {
                return Resulte<ProductDTO>.Failure("There is a order items that referenc to product");
            }

            await _productsReposetory.DeleteProductsReposetory(id);
            return Resulte<ProductDTO>.Success(null);
        }
    }
}
