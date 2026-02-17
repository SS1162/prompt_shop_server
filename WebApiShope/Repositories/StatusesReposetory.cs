using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;


namespace Repositories
{
    public class StatusesReposetory : IStatusesReposetory
    {
        MyShop330683525Context _DBContext;
        public StatusesReposetory(MyShop330683525Context DBContext)
        {
            this._DBContext = DBContext;
        }

        async public Task<Status?> GetStatusByID(long statusID)
        {
            return await _DBContext.Statuses.FirstOrDefaultAsync(x => x.StatusId == statusID);
        }

    }
}
