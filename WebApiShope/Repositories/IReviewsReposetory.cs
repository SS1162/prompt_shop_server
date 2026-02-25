using Entities;
using System.Collections.Generic;

namespace Repositories
{
    public interface IReviewsReposetory
    {
        Task<Review> AddReviewReposetory(Review review);
        Task<Review> GetReviewByOrderIdReposetory(long orderId);
        Task UpdateReviewReposetory(long id ,Review review);
        Task<Review?> GetByidReviewReposetory(long id);
        Task<IEnumerable<Review>> GetAllReviewsReposetory(int limit, int currentPage);
    }
}