using DTO;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using AutoMapper;
namespace Services
{
    public class MainCategoriesServise : IMainCategoriesServise
    {

        private readonly IMainCategoriesReposetory _mainCategoryReposetory;
        private readonly ICategoriesReposetory _categoriesReposetory;
        private readonly IMapper _mapper;
        public MainCategoriesServise(IMainCategoriesReposetory mainCategoryReposetory, IMapper mapper, ICategoriesReposetory categoriesReposetory)
        {
            this._mainCategoryReposetory = mainCategoryReposetory;
            this._mapper = mapper;
            this._categoriesReposetory = categoriesReposetory;

        }
        async public Task<IEnumerable<MainCategoriesDTO>> GetMainCategoriesServises()
        {
            IEnumerable<MainCategory> MainCategoriesListFromReposetory = await _mainCategoryReposetory.GetMainCategoriesReposetoty();
            IEnumerable<MainCategoriesDTO> MainCategoriesListToController = _mapper.Map<IEnumerable<MainCategoriesDTO>>(MainCategoriesListFromReposetory);
            return MainCategoriesListToController;
        }


        async public Task<MainCategoriesDTO> AddMainCategoriesServises(ManegerMainCategoryDTO manegerMainCategory)
        {
            MainCategory mainCategoryToReposetory = _mapper.Map<MainCategory>(manegerMainCategory);
            //הכנסה של פרומפט ע"י gemini
            MainCategory mainCategoryFromeposetory = await _mainCategoryReposetory.AddMainCategoriesReposetoty(mainCategoryToReposetory);
            return _mapper.Map<MainCategoriesDTO>(mainCategoryFromeposetory);
        }

        async public Task<Resulte<MainCategoriesDTO>> UpdateMainCategoriesServises(long id, MainCategoriesDTO MainCategoriesFromController)
        {
            if (id != MainCategoriesFromController.MainCategoryID)
            {
                return Resulte<MainCategoriesDTO>.Failure("The ids are diffrent");
            }

            MainCategory? checkIfMainCategoryInsist = await _mainCategoryReposetory.GetByIdMainCategoriesReposetoty(id);

            if (checkIfMainCategoryInsist == null)
            {
                return Resulte<MainCategoriesDTO>.Failure("The main category's id is not found");
            }

            MainCategory mainCategoryToReposetory = _mapper.Map<MainCategory>(MainCategoriesFromController);
            //הכנסה של פרומפט ע"י gemini

            await _mainCategoryReposetory.UpdateMainCategoriesReposetoty(id, mainCategoryToReposetory);
            return Resulte<MainCategoriesDTO>.Success(null);
        }

        async public Task<Resulte<MainCategoriesDTO>> DeleteMainCategoriesServises(long id)
        {
            Category? checkIfThereIsCategories = await _categoriesReposetory.GetByMainCategoriesIDReposetory(id);
            if (checkIfThereIsCategories != null)
            {
                Resulte<MainCategoriesDTO>.Failure("The is categories that refernce to that main category");
            }

            MainCategory? checkIfMainCategoriesInsist = await _mainCategoryReposetory.GetByIdMainCategoriesReposetoty(id);
            if (checkIfMainCategoriesInsist != null)
            {
                Resulte<MainCategoriesDTO>.Failure("The main categoty not found ");
            }
            await _mainCategoryReposetory.DeleteMainCategoriesReposetoty(id);
            return Resulte<MainCategoriesDTO>.Success(null);

        }
    }
}
