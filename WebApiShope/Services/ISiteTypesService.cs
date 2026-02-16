using DTO;

namespace Services
{
    public interface ISiteTypesService
    {
        Task<IEnumerable<SiteTypeDTO>?> GetAllSiteTypesServise();
        Task<SiteTypeDTO?> GetSiteTypesByIdServise(long id);
        Task<Resulte<SiteTypeDTO>> UpdateSiteTypesByMngServise(long id, SiteTypeDTO dto);
    }
}