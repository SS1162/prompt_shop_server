using Entities;

namespace Repositories
{
    public interface IGeminiPromptsReposetory
    {
        Task<GeminiPrompt> AddPromptReposetory(GeminiPrompt prompt);
        Task UpdatePromptReposetory(long id, GeminiPrompt prompt);

        Task<GeminiPrompt?> GetByIDPromptReposetory(long id);
    }
}