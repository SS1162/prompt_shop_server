
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;
namespace Repositories
{
    public class PlatformsReposetory : IPlatformsReposetory
    {
        private readonly MyShop330683525Context _DBContext;
        public PlatformsReposetory(MyShop330683525Context _DBContext)
        {
            this._DBContext = _DBContext;
        }

        async public Task<Platform?> GetByIDPlatformsReposetory(long id)
        {
            return await _DBContext.Platforms.AsNoTracking().FirstOrDefaultAsync(x => x.PlatformId == id);
        }

        async public Task<IEnumerable<Platform>> GetPlatformsReposetory()
        {
            return await _DBContext.Platforms.ToListAsync();
        }

        async public Task<Platform> AddPlatformReposetory(Platform platform)
        {
            await _DBContext.Platforms.AddAsync(platform);
            await _DBContext.SaveChangesAsync();
            return platform;
        }

        async public Task UpdatePlatformReposetory(long id, Platform platform)
        {
            _DBContext.Platforms.Update(platform);
            await _DBContext.SaveChangesAsync();
        }

        async public Task DeletePlatformReposetory(long id)
        {
            Platform platformObjectToDelete = await _DBContext.Platforms.FirstOrDefaultAsync(x => x.PlatformId == id);
            _DBContext.Platforms.Remove(platformObjectToDelete);
            await _DBContext.SaveChangesAsync();
        }
    }
}
