using DTO;
using Entities;

namespace Services
{
    public interface IGeminiServise
    {
        Task<Resulte<GeminiPromptDTO>> AddGeminiForUserProductServise(long categoryId, string userRequest);
        Task<Resulte<GeminiPromptDTO>> AddGeminiForUserFillCategoryServise(string userRequest, long categoryId);

        Task<Resulte<GeminiPromptDTO>> AddGeminiForUserFillBasicSiteServise(string userRequest);

        Task<Resulte<GeminiPrompt>> UpdateGeminiForUserProductServise(long promptId, string userRequest);

        Task<Resulte<GeminiPrompt>> UpdateGeminiForUserCategoryServise(long promptId, string userRequest);

        Task<Resulte<GeminiPrompt>> UpdateGeminiForUserBasicSiteServise(long promptId, string userRequest);

        Task<GeminiPromptDTO?> GetByIdPromptServise(long promptId);

        Task<Resulte<GeminiPrompt>> DeletePromptServise(long promptId);


    }
}