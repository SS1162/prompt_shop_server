using DTO;

namespace Services
{
    public interface IPlatformsServise
    {
        Task<PlatformsDTO> AddPlatformServise(AddPlatformDTO platformToAdd);
        Task<Resulte<PlatformsDTO>> DeletePlatformServise(long id);
        Task<IEnumerable<PlatformsDTO>> GetPlatformsServise();
        Task<Resulte<PlatformsDTO>> UpdatePlatformServise(long id, PlatformsDTO platform);
    }
}