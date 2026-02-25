using Entities;

namespace Repositories
{
    public interface IPlatformsReposetory
    {
        Task<Platform> AddPlatformReposetory(Platform platform);
        Task DeletePlatformReposetory(long id);
        Task<IEnumerable<Platform>> GetPlatformsReposetory();
        Task UpdatePlatformReposetory(long id, Platform platform);
        Task<Platform?> GetByIDPlatformsReposetory(long id);

    }
}