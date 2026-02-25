using DTO;

namespace Services
{
    public interface IReviewsServise
    {
        Task<Resulte<ReviewDTO>> AddReviewServise(long orderId, AddReviewDTO review);
        Task<Resulte<ReviewDTO>> GetReviewByOrderIdServise(long orderId);
        Task<Resulte<ReviewDTO>> UpdateReviewServise(long id, ReviewDTO review);
        Task<Resulte<IEnumerable<ReviewDTO>>> GetAllReviewsServise(int limit, int currentPage);
    }
}