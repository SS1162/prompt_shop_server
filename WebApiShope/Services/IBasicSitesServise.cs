using DTO;

namespace Services
{
    public interface IBasicSitesServise
    {
        Task<Resulte<BasicSiteDTO?>> AddBasicSiteServise(AddBasicSiteDTO BasicSiteToAdd);
        Task<BasicSiteDTO> GetByIDbasicSiteServise(long id);
        Task<Resulte<BasicSiteDTO?>> UpdateBasicSiteServise(long id, UpdateBasicSiteDTO basicSiteToUpdate);
    }
}