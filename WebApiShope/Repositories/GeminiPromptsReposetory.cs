using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class GeminiPromptsReposetory : IGeminiPromptsReposetory
    {
        private readonly MyShop330683525Context _DBContext;
        public GeminiPromptsReposetory(MyShop330683525Context DBContext)
        {
            this._DBContext = DBContext;
        }

        async public Task<GeminiPrompt> AddPromptReposetory(GeminiPrompt prompt)
        {
            await _DBContext.GeminiPrompts. AddAsync(prompt);
               
            await _DBContext.SaveChangesAsync();
            return prompt;
        }



        async public Task UpdatePromptReposetory(long id, GeminiPrompt prompt)
        {
            _DBContext.GeminiPrompts.Update(prompt);
            await _DBContext.SaveChangesAsync();

        }

        async public Task DeletePromptReposetory(long id)
        {
            GeminiPrompt prompt = await _DBContext.GeminiPrompts.FirstOrDefaultAsync(x => x.PromptId == id);
            _DBContext.GeminiPrompts.Remove(prompt);
            await _DBContext.SaveChangesAsync();

        }
        async public Task<GeminiPrompt?> GetByIDPromptReposetory(long id)
        {
            return await _DBContext.GeminiPrompts.FirstOrDefaultAsync(x => x.PromptId == id);
       

        }
    }
}
