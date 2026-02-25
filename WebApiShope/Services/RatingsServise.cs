using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Repositories;
namespace Services
{
    public class RatingsServise : IRatingsServise
    {
        private readonly IRatingsReposetory _ratingsReposetory;
        public RatingsServise(IRatingsReposetory ratingsReposetory)
        {
            _ratingsReposetory = ratingsReposetory;
        }

        public async Task<Rating> AddRatingServise(Rating ratingToAdd)
        {
            return await _ratingsReposetory.AddRatingReposetory(ratingToAdd);
        }
    }
}
