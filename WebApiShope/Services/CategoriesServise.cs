using AutoMapper;
using DTO;
using Entities;
using Google.GenAI.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repositories;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Services

{
    public class CategoriesServise : ICategoriesServise
    {
        private readonly ICategoriesReposetory _categoriesReposetory;
        private readonly IMapper _mapper;
        private readonly IMainCategoriesReposetory _mainCategoriesReposetory;
        private readonly IProductsReposetory _productsReposetory;
        private readonly IConfiguration _config;
        public CategoriesServise(ICategoriesReposetory categoriesReposetory, IMapper mapper,
              IMainCategoriesReposetory mainCategoriesReposetory, IProductsReposetory productsReposetory,
              IConfiguration config)
        {
            this._mapper = mapper;
            this._categoriesReposetory = categoriesReposetory;
            this._mainCategoriesReposetory = mainCategoriesReposetory;
            this._productsReposetory = productsReposetory;
            this._config=config;    
        }

        async public Task<Resulte<ResponePage<CategoryDTO>>> GetCategoriesServise(int numberOfPages, long mainCategoryID, int pageSize, string? search)
        {
            MainCategory? checkIfMainCategoryInsist = await _mainCategoriesReposetory.GetByIdMainCategoriesReposetoty(mainCategoryID);
            if (checkIfMainCategoryInsist == null)
            {
                Resulte<ResponePage<CategoryDTO>>.Failure("Main category id isn't insist");
            }
            if (pageSize > 50)
            {
                Resulte<ResponePage<CategoryDTO>>.Failure("The page Size is too big");
            }
            (IEnumerable<Category>, int) responeFromReposetory = await _categoriesReposetory.GetCategoriesReposetory(numberOfPages, mainCategoryID, pageSize, search);

            ResponePage<CategoryDTO> responeToClient = new ResponePage<CategoryDTO>();
            responeToClient.Data = _mapper.Map<IEnumerable<CategoryDTO>>(responeFromReposetory.Item1);
            responeToClient.TotalItems = responeFromReposetory.Item2;
            responeToClient.CurrentPage = numberOfPages;
            responeToClient.PageSize = pageSize;
            responeToClient.HasPreviousPage = numberOfPages > 0;
            responeToClient.HasNextPage = (numberOfPages - 1) * pageSize > responeFromReposetory.Item2;
            return Resulte<ResponePage<CategoryDTO>>.Success(responeToClient);
        }

        async public Task<CategoryDTO> GetByIDCategoriesServise(long id)
        {
            Category? CategoryFromReposetory = await _categoriesReposetory.GetByIDCategoriesReposetory(id);
            return _mapper.Map<CategoryDTO>(CategoryFromReposetory);
        }

        async public Task<Resulte<CategoryDTO?>> UpdateCategoriesServise(long id, CategoryToUpdateDTO categoryToUpdate)
        {
            if (id != categoryToUpdate.CategoryID)
            {
                Resulte<CategoryDTO>.Failure("The ids are diffrent");
            }
            MainCategory? checkIfMainCategoryInsist = await _mainCategoriesReposetory.GetByIdMainCategoriesReposetoty(categoryToUpdate.MainCategoryID);
            if (checkIfMainCategoryInsist == null)
            {
                Resulte<CategoryDTO>.Failure("Main category id isn't insist");
            }

            Category? checkIfCategoryInsist = await _categoriesReposetory.GetByIDCategoriesReposetory(id);
            if (checkIfCategoryInsist == null)
            {
                Resulte<CategoryDTO>.Failure("Category id isn't insist");
            }
            Category categoryToReposetory = _mapper.Map<Category>(categoryToUpdate);
            //למלא פרומפט עם gemini
            categoryToReposetory.CategoryPrompt = "vfsghhfg";
            await _categoriesReposetory.UpdateCategoriesReposetory(id, categoryToReposetory);
            if (System.IO.File.Exists(checkIfCategoryInsist.ImgUrl))
            {

                System.IO.File.Delete(checkIfCategoryInsist.ImgUrl);
            }

            using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(categoryToUpdate.ImgUrl.OpenReadStream()))
            {

                ResizeOptions options = new ResizeOptions
                {
                    Size = new Size(800, 0)
                };


                image.Mutate(processor => processor.Resize(options));


                JpegEncoder encoder = new JpegEncoder
                {
                    Quality = 75
                };

                string fullPath = _config.GetValue<string>("IMAGES_CATEGORIES_PATH") + categoryToUpdate.ImgUrl + ".jpeg";
                await image.SaveAsync(fullPath, encoder);

            }

            return Resulte<CategoryDTO>.Success(null);
        }

        async public Task<Resulte<CategoryDTO>> AddCategoriesServise(AddCategoryDTO categoryToAdd)
        {


            MainCategory? checkIfMainCategoryInsist = await _mainCategoriesReposetory.GetByIdMainCategoriesReposetoty(categoryToAdd.MainCategoryID);
            if (checkIfMainCategoryInsist == null)
            {
                Resulte<CategoryDTO>.Failure("Main category id isn't insist");
            }
            Category categoryToReposetory = _mapper.Map<Category>(categoryToAdd);


            using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(categoryToAdd.ImgUrl.OpenReadStream()))
            {
 
                ResizeOptions options = new ResizeOptions
                 {
                     Size = new Size(800, 0)
                    };


                image.Mutate(processor => processor.Resize(options));


                JpegEncoder encoder = new JpegEncoder
                {
                    Quality = 75
                };

                string fullPath = _config.GetValue<string>("IMAGES_CATEGORIES_PATH") + categoryToAdd.ImgUrl+".jpeg";
                await image.SaveAsync(fullPath, encoder);


            }
            categoryToReposetory.ImgUrl =  _config.GetValue<string>("IMAGES_CATEGORIES_PATH")+ categoryToReposetory.ImgUrl ;
            //למלא פרומפט עם gemini
            categoryToReposetory.CategoryPrompt = "gfasdfghfh";
            Category categoryFromReposetory = await _categoriesReposetory.AddCategoriesReposetory(categoryToReposetory);

            return Resulte<CategoryDTO>.Success(_mapper.Map<CategoryDTO>(categoryFromReposetory));
        }
        async public Task<Resulte<CategoryDTO>> DeleteIDCategoriesServise(long id)
        {
            Product? checkIfProductInsist = await _productsReposetory.HasProductsToCatrgoryReposetory(id);
            if (checkIfProductInsist != null)
            {
                Resulte<CategoryDTO>.Failure("There is product that reference to this category");
            }
            Category? checkIfCategoryInsist = await _categoriesReposetory.GetByIDCategoriesReposetory(id);
            if (checkIfCategoryInsist == null)
            {
                Resulte<CategoryDTO>.Failure("Category id isn't insist");
            }
            await _categoriesReposetory.DeleteIDCategoriesReposetory(id);
            if (System.IO.File.Exists(checkIfCategoryInsist.ImgUrl))
            {
             
                System.IO.File.Delete(checkIfCategoryInsist.ImgUrl);
            }
            return Resulte<CategoryDTO>.Success(null);
        }

    }
}
