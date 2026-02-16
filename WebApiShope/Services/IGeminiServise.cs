using DTO;
using Entities;

namespace Services
{
    public interface IGeminiServise
    {
        Task<Resulte<GeminiPrompt>> AddGeminiForUserProductServise(long categoryId, string userRequest);
        Task<Resulte<GeminiPrompt>> AddGeminiForUserFillCategoryServise(string userRequest, long categoryId);

        Task<Resulte<GeminiPrompt>> AddGeminiForUserFillBasicSiteServise(string userRequest);

        Task<Resulte<GeminiPrompt>> UpdateGeminiForUserProductServise(long promptId, string userRequest);

        Task<Resulte<GeminiPrompt>> UpdateGeminiForUserCategoryServise(long promptId, string userRequest);

        Task<Resulte<GeminiPrompt>> UpdateGeminiForUserBasicSiteServise(long promptId, string userRequest);

        Task<GeminiPrompt?> GetByIdPromptServise(long promptId);

        Task<Resulte<GeminiPrompt>> DeletePromptServise(long promptId);


    }
}