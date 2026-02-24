using AutoMapper;
using DTO;
using Entities;
using Microsoft.Identity.Client;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
    public class GeminiServise : IGeminiServise
    {
        private readonly Igemini _gemini;
        private readonly IGeminiPromptsReposetory _geminiPromptsReposetory;
        private readonly ICategoriesReposetory _categoriesReposetory;
        private readonly IMainCategoriesReposetory _mainCategoriesReposetory;
        private readonly IMapper _mapper;
        public GeminiServise(Igemini gemini, ICategoriesReposetory categoriesReposetory,
            IGeminiPromptsReposetory geminiPromptsReposetory, IMainCategoriesReposetory mainCategoriesReposetory ,IMapper mapper   )
        {
            this._gemini = gemini;
            this._categoriesReposetory = categoriesReposetory;
            this._geminiPromptsReposetory = geminiPromptsReposetory;
            this._mainCategoriesReposetory = mainCategoriesReposetory;
            this._mapper = mapper;
        }

        public async Task<Resulte<GeminiPromptDTO>> AddGeminiForUserProductServise(long categoryId, string userRequest)
        {
            Category? category = await _categoriesReposetory.GetByIDCategoriesReposetory(categoryId);
            if (category == null)
            {
               return  Resulte<GeminiPromptDTO>.Failure("The product id is incorect");
            }
            Resulte<string> resulte = await _gemini.RunGeminiForUserProduct(userRequest, category.CategoryName);
            if(!resulte.IsSuccess)
            {
              return   Resulte<GeminiPromptDTO>.Failure("Server error");
            }

            if(resulte.Data==null)
            {
                return Resulte<GeminiPromptDTO>.Failure("Server error");
            }
            GeminiPrompt prompt = new GeminiPrompt();
            string jsonString = resulte.Data;
            Technical_value paseJSON = JsonSerializer.Deserialize<Technical_value>(jsonString);
            prompt.Prompt = paseJSON.technical_value;
            prompt.CategoryId = categoryId;
           
            GeminiPrompt resulteFromReposetory = await _geminiPromptsReposetory.AddPromptReposetory(prompt);

            GeminiPromptDTO promptForReturn = _mapper.Map<GeminiPromptDTO>(resulteFromReposetory);
            return Resulte<GeminiPromptDTO>.Success(promptForReturn);
        }




        public async Task<Resulte<GeminiPromptDTO>> AddGeminiForUserFillCategoryServise(string userRequest, long categoryId)
        {
            Category? category = await _categoriesReposetory.GetByIDCategoriesReposetory(categoryId);
            if (category == null)
            {
               return  Resulte<GeminiPromptDTO>.Failure("The product id is incorect");
            }


            MainCategory? mainCategory = await _mainCategoriesReposetory.GetByIdMainCategoriesReposetoty(category.MainCategoryId);
            if (mainCategory == null)
            {
               return  Resulte<GeminiPromptDTO>.Failure("The main category id is incorect");
            }
         
            Resulte<string> resulte = await _gemini.RunGeminiForFillCategory(userRequest, mainCategory.MainCategoryName);
            if (!resulte.IsSuccess)
            {
                return Resulte<GeminiPromptDTO>.Failure(resulte.ErrorMessage);
            }

            if (resulte.Data == null)
            {
                Resulte<GeminiPrompt>.Failure("Server error");
            }
            GeminiPrompt prompt = new GeminiPrompt();
            string jsonString = resulte.Data;
            Technical_value paseJSON = JsonSerializer.Deserialize<Technical_value>(jsonString);
            prompt.Prompt = paseJSON.technical_value;
            
            prompt.CategoryId = categoryId;

            GeminiPrompt resulteFromReposetory = await _geminiPromptsReposetory.AddPromptReposetory(prompt);

            GeminiPromptDTO promptForReturn = _mapper.Map<GeminiPromptDTO>(resulteFromReposetory);
            return Resulte<GeminiPromptDTO>.Success(promptForReturn);
        }

        public async Task<Resulte<GeminiPromptDTO>> AddGeminiForUserFillBasicSiteServise(string userRequest)
        {
            
            Resulte<string> resulte = await _gemini.RunGeminiForFillBasicSite(userRequest);
            if (!resulte.IsSuccess)
            {
                return Resulte<GeminiPromptDTO>.Failure(resulte.ErrorMessage);
            }

            if (resulte.Data == null)
            {
               return Resulte<GeminiPromptDTO>.Failure("Server error");
            }
            GeminiPrompt prompt = new GeminiPrompt();
            string jsonString =resulte.Data;
            Technical_value paseJSON = JsonSerializer.Deserialize<Technical_value>(jsonString);
            prompt.Prompt = paseJSON.technical_value;

            GeminiPrompt resulteFromReposetory = await _geminiPromptsReposetory.AddPromptReposetory(prompt);
            GeminiPromptDTO promptForReturn = _mapper.Map<GeminiPromptDTO>(resulteFromReposetory);
            return Resulte<GeminiPromptDTO>.Success(promptForReturn);
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
            string jsonString = resulte.Data;
            Technical_value paseJSON = JsonSerializer.Deserialize<Technical_value>(jsonString);
            checkIfThePromptExist.Prompt = paseJSON.technical_value;
   
          
               await _geminiPromptsReposetory.UpdatePromptReposetory(promptId, checkIfThePromptExist);
                return Resulte<GeminiPrompt>.Success(null);
        }


        public async Task<Resulte<GeminiPrompt>> UpdateGeminiForUserCategoryServise(long promptId, string userRequest)
        {
            GeminiPrompt? checkIfThePromptExist = await _geminiPromptsReposetory.GetByIDPromptReposetory(promptId);

            if (checkIfThePromptExist == null)
            {
               return  Resulte<GeminiPrompt>.Failure("The prompt id is incorect");
            }

        

             Category? category = await _categoriesReposetory.GetByIDCategoriesReposetory((long)checkIfThePromptExist.CategoryId);
           
            if (category == null)
            {
                return Resulte<GeminiPrompt>.Failure("The product id is incorect");
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
            string jsonString = resulte.Data;
            Technical_value paseJSON = JsonSerializer.Deserialize<Technical_value>(jsonString);
            checkIfThePromptExist.Prompt = paseJSON.technical_value;

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
            string jsonString = resulte.Data;
            Technical_value paseJSON = JsonSerializer.Deserialize<Technical_value>(jsonString);
            checkIfThePromptExist.Prompt = paseJSON.technical_value;

            await _geminiPromptsReposetory.UpdatePromptReposetory(promptId, checkIfThePromptExist);
            return Resulte<GeminiPrompt>.Success(null);
        }
        public async Task<GeminiPromptDTO?> GetByIdPromptServise(long promptId)
        {
           return  _mapper.Map< GeminiPromptDTO> (await _geminiPromptsReposetory.GetByIDPromptReposetory(promptId));
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