using Entities;

namespace Repositories
{
    public interface IStatusesReposetory
    {
        Task<Status?> GetStatusByID(long statusID);
    }
}