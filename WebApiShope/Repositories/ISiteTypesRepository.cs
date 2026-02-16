using Entities;

namespace Repositories
{
    public interface ISiteTypesRepository
    {
        Task<IEnumerable<SiteType>?> GetAllSiteTypesReposetory();
        Task<SiteType?> GetSiteTypeByIdReposetory(long id);
        Task UpdateSiteTypeByMngReposetory(long id, SiteType siteType);
    }
}