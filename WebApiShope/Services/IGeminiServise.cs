using DTO;
using Entities;

namespace Services
{
    public interface IGeminiServise
    {
        Task<Resulte<GeminiPrompt>> AddGeminiForUserProductServise(long categoryId, string userRequest);
        Task<Resulte<GeminiPrompt>> UdateGeminiForUserProductServise(long promptId, string userRequest);

        Task<GeminiPrompt?> GetByIdPromptServise(long promptId);
    }
}