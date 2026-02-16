using Entities;

namespace Repositories
{
    public interface IBasicSitesReposetory
    {
        Task<BasicSite> AddBasicSiteReposetory(BasicSite basicSiteToUpdate);
        Task<BasicSite?> GetByIDBasicSiteReposetory(long id);
        Task UpdateBasicSiteReposetory(long id, BasicSite basicSiteToUpdate);
        public Task<BasicSite?> CheckIfHasPlatformByPlatformID(long id);
    }
}