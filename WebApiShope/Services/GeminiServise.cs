using DTO;
using Entities;
using Microsoft.Identity.Client;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class GeminiServise : IGeminiServise
    {
        Igemini _gemini;
        IGeminiPromptsReposetory _geminiPromptsReposetory;
        ICategoriesReposetory _categoriesReposetory;
        IMainCategoriesReposetory _mainCategoriesReposetory;
        public GeminiServise(Igemini gemini, ICategoriesReposetory categoriesReposetory,
            IGeminiPromptsReposetory geminiPromptsReposetory, IMainCategoriesReposetory mainCategoriesReposetory    )
        {
            this._gemini = gemini;
            this._categoriesReposetory = categoriesReposetory;
            this._geminiPromptsReposetory = geminiPromptsReposetory;
            this._mainCategoriesReposetory = mainCategoriesReposetory;
        }

        public async Task<Resulte<GeminiPrompt>> AddGeminiForUserProductServise(long categoryId, string userRequest)
        {
            Category? category = await _categoriesReposetory.GetByIDCategoriesReposetory(categoryId);
            if (category == null)
            {
                Resulte<GeminiPrompt>.Failure("The product id is incorect");
            }
            Resulte<string> resulte = await _gemini.RunGeminiForUserProduct(userRequest, category.CategoryName);
            if(!resulte.IsSuccess)
            {
                Resulte<GeminiPrompt>.Failure("Server error");
            }

            if(resulte.Data==null)
            {
                Resulte<GeminiPrompt>.Failure("Server error");
            }
            GeminiPrompt prompt = new GeminiPrompt();
            prompt.Prompt = resulte.Data;
            prompt.CategoryId = categoryId;
           
            GeminiPrompt resulteFromReposetory = await _geminiPromptsReposetory.AddPromptReposetory(prompt);
             return Resulte<GeminiPrompt>.Success(resulteFromReposetory);
        }




        public async Task<Resulte<GeminiPrompt>> AddGeminiForUserFillCategoryServise(string userRequest, long categoryId)
        {
            Category? category = await _categoriesReposetory.GetByIDCategoriesReposetory(categoryId);
            if (category == null)
            {
                Resulte<GeminiPrompt>.Failure("The product id is incorect");
            }


            MainCategory? mainCategory = await _mainCategoriesReposetory.GetByIdMainCategoriesReposetoty(category.MainCategoryId);
            if (mainCategory == null)
            {
                Resulte<GeminiPrompt>.Failure("The main category id is incorect");
            }
         
            Resulte<string> resulte = await _gemini.RunGeminiForFillCategory(userRequest, mainCategory.MainCategoryName);
            if (!resulte.IsSuccess)
            {
                Resulte<GeminiPrompt>.Failure(resulte.ErrorMessage);
            }

            if (resulte.Data == null)
            {
                Resulte<GeminiPrompt>.Failure("Server error");
            }
            GeminiPrompt prompt = new GeminiPrompt();
            prompt.Prompt = resulte.Data;
            prompt.CategoryId = categoryId;

            GeminiPrompt resulteFromReposetory = await _geminiPromptsReposetory.AddPromptReposetory(prompt);
            return Resulte<GeminiPrompt>.Success(resulteFromReposetory);
        }

        public async Task<Resulte<GeminiPrompt>> AddGeminiForUserFillBasicSiteServise(string userRequest)
        {
            
            Resulte<string> resulte = await _gemini.RunGeminiForFillBasicSite(userRequest);
            if (!resulte.IsSuccess)
            {
                Resulte<GeminiPrompt>.Failure(resulte.ErrorMessage);
            }

            if (resulte.Data == null)
            {
                Resulte<GeminiPrompt>.Failure("Server error");
            }
            GeminiPrompt prompt = new GeminiPrompt();
            prompt.Prompt = resulte.Data;

            GeminiPrompt resulteFromReposetory = await _geminiPromptsReposetory.AddPromptReposetory(prompt);
            return Resulte<GeminiPrompt>.Success(resulteFromReposetory);
        }


        public async Task<Resulte<GeminiPrompt>> UpdateGeminiForUserProductServise(long promptId, string userRequest)
        {
            GeminiPrompt? checkIfThePromptExist = await _geminiPromptsReposetory.GetByIDPromptReposetory(promptId);

            if (checkIfThePromptExist == null)
            {
                return Resulte<GeminiPrompt>.Failure("The prompt id is incorect");
            }
         
            Category? category = await _categoriesReposetory.GetByIDCategoriesReposetory(Convert.ToInt32(checkIfThePromptExist.CategoryId));
            Resulte<string> resulte = await _gemini.RunGeminiForUserProduct(userRequest, category.CategoryName);
            if(!resulte.IsSuccess)
            {
                return Resulte<GeminiPrompt>.Failure("Server error");
            }
            if(resulte.Data==null)
            {
                return Resulte<GeminiPrompt>.Failure("Server error");
            }
            checkIfThePromptExist.Prompt = resulte.Data;
          
               await _geminiPromptsReposetory.UpdatePromptReposetory(promptId, checkIfThePromptExist);
                return Resulte<GeminiPrompt>.Success(null);
        }


        public async Task<Resulte<GeminiPrompt>> UpdateGeminiForUserCategoryServise(long promptId, string userRequest)
        {
            GeminiPrompt? checkIfThePromptExist = await _geminiPromptsReposetory.GetByIDPromptReposetory(promptId);

            if (checkIfThePromptExist == null)
            {
                Resulte<GeminiPrompt>.Failure("The prompt id is incorect");
            }

        

             Category? category = await _categoriesReposetory.GetByIDCategoriesReposetory((long)checkIfThePromptExist.CategoryId);
           
            if (category == null)
            {
                Resulte<GeminiPrompt>.Failure("The product id is incorect");
            }


            MainCategory? mainCategory = await _mainCategoriesReposetory.GetByIdMainCategoriesReposetoty(category.MainCategoryId);
            if (mainCategory == null)
            {
                return Resulte<GeminiPrompt>.Failure("The main category id is incorect");
            }
          

            Resulte<string> resulte = await _gemini.RunGeminiForFillCategory(userRequest, category.CategoryName);
            if (!resulte.IsSuccess)
            {
                return Resulte<GeminiPrompt>.Failure("Server error");
            }
            if (resulte.Data == null)
            {
                return Resulte<GeminiPrompt>.Failure("Server error");
            }
            checkIfThePromptExist.Prompt = resulte.Data;

            await _geminiPromptsReposetory.UpdatePromptReposetory(promptId, checkIfThePromptExist);
            return Resulte<GeminiPrompt>.Success(null);
        }

        public async Task<Resulte<GeminiPrompt>> UpdateGeminiForUserBasicSiteServise(long promptId,string userRequest)
        {
            GeminiPrompt? checkIfThePromptExist = await _geminiPromptsReposetory.GetByIDPromptReposetory(promptId);

            if (checkIfThePromptExist == null)
            {
                return Resulte<GeminiPrompt>.Failure("The prompt id is incorect");
            }

            Resulte<string> resulte = await _gemini.RunGeminiForFillBasicSite(userRequest);
            if (!resulte.IsSuccess)
            {
                return Resulte<GeminiPrompt>.Failure("Server error");
            }
            if (resulte.Data == null)
            {
                return Resulte<GeminiPrompt>.Failure("Server error");
            }
            checkIfThePromptExist.Prompt = resulte.Data;

            await _geminiPromptsReposetory.UpdatePromptReposetory(promptId, checkIfThePromptExist);
            return Resulte<GeminiPrompt>.Success(null);
        }
        public async Task<GeminiPrompt?> GetByIdPromptServise(long promptId)
        {
           return  await _geminiPromptsReposetory.GetByIDPromptReposetory(promptId);
        }

        public async Task<Resulte<GeminiPrompt>> DeletePromptServise(long promptId)
        {
            GeminiPrompt? checkIfThePromptExist = await _geminiPromptsReposetory.GetByIDPromptReposetory(promptId);
            if(checkIfThePromptExist==null)
            {
                return Resulte<GeminiPrompt>.Failure("The prompt id is incorect");
            }
            await _geminiPromptsReposetory.DeletePromptReposetory(promptId);
            return Resulte<GeminiPrompt>.Success(null);
        }

    }
}