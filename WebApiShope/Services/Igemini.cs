
using DTO;
using Entities;

namespace Services
{
    public interface Igemini
    {
        Task<Resulte<string>> RunGeminiForUserProduct(string userRequest, string category);

        Task<Resulte<string>> RunGeminiForFillCategory(string userRequest, string mainCategory);

        Task<Resulte<string>> RunGeminiForFillBasicSite(string userRequest);
    }
}