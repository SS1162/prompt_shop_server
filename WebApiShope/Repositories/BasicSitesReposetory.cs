using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class BasicSitesReposetory : IBasicSitesReposetory
    {
        private readonly MyShop330683525Context _DBContext;
        public BasicSitesReposetory(MyShop330683525Context DBContext)
        {
            this._DBContext = DBContext;
        }
        async public Task<BasicSite?> GetByIDBasicSiteReposetory(long id)
        {
            return await _DBContext.BasicSites.Include(x => x.BasicSitesPlatforms)
                .Include(x => x.SiteType)
                .FirstOrDefaultAsync(x => x.BasicSiteId == id);

        }

        async public Task<BasicSite?> CheckIfHasPlatformByPlatformID(long id)
        {
            return await _DBContext.BasicSites.FirstOrDefaultAsync(x => x.BasicSitesPlatforms == id);
          

        }

        async public Task UpdateBasicSiteReposetory(long id, BasicSite basicSiteToUpdate)
        {
            _DBContext.BasicSites.Update(basicSiteToUpdate);
            await _DBContext.SaveChangesAsync();

        }



        async public Task<BasicSite> AddBasicSiteReposetory(BasicSite basicSiteToUpdate)
        {
            await _DBContext.BasicSites.AddAsync(basicSiteToUpdate);
            await _DBContext.SaveChangesAsync();
            return basicSiteToUpdate;

        }
    }
}
