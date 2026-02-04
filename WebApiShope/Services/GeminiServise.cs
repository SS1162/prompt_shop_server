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
        public GeminiServise(Igemini gemini, ICategoriesReposetory categoriesReposetory,
            IGeminiPromptsReposetory geminiPromptsReposetory)
        {
            this._gemini = gemini;
            this._categoriesReposetory = categoriesReposetory;
            this._geminiPromptsReposetory = geminiPromptsReposetory;
        }

        public async Task<Resulte<GeminiPrompt>> AddGeminiForUserProductServise(long categoryId, string userRequest)
        {
            Category? category = await _categoriesReposetory.GetByIDCategoriesReposetory(categoryId);
            if (category == null)
            {
                Resulte<GeminiPrompt>.Failure("The product id is incorect");
            }
            string resulte = await _gemini.RunGeminiForUserProduct(userRequest, category.CategoryName);
            GeminiPrompt prompt = new GeminiPrompt();
            prompt.Prompt = resulte;
            prompt.CategoryId = categoryId;
            if(resulte!=null)
            {
                GeminiPrompt resulteFronReposetory = await _geminiPromptsReposetory.AddPromptReposetory(prompt);
                 return Resulte<GeminiPrompt>.Success(resulteFronReposetory);
            }
           return Resulte<GeminiPrompt>.Success(prompt);
           
        }




        public async Task<Resulte<GeminiPrompt>> UdateGeminiForUserProductServise(long promptId, string userRequest)
        {
            GeminiPrompt? checkIfThePromptExist = await _geminiPromptsReposetory.GetByIDPromptReposetory(promptId);

            if (checkIfThePromptExist == null)
            {
                Resulte<GeminiPrompt>.Failure("The prompt id is incorect");
            }
            Category? category = await _categoriesReposetory.GetByIDCategoriesReposetory(checkIfThePromptExist.CategoryId);
            string resulte = await _gemini.RunGeminiForUserProduct(userRequest, category.CategoryName);
            checkIfThePromptExist.Prompt = resulte;
            if(resulte!=null)
            {
                await _geminiPromptsReposetory.UpdatePromptReposetory(promptId, checkIfThePromptExist);

            }
           
            return Resulte<GeminiPrompt>.Success(null);
        }
        


        public async Task<GeminiPrompt?> GetByIdPromptServise(long promptId)
        {
           return  await _geminiPromptsReposetory.GetByIDPromptReposetory(promptId);
        }
    }
}