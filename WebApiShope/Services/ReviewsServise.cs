using AutoMapper;
using DTO;
using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ReviewsServise : IReviewsServise
    {
        private readonly IReviewsReposetory _reviewsReposetory;
        private readonly IMapper _mapper;
        private readonly IOrdersReposetory _ordersReposetory;


        public ReviewsServise(IReviewsReposetory reviewsReposetory, IMapper mapper, IOrdersReposetory ordersReposetory)
        {
            this._reviewsReposetory = reviewsReposetory;
            this._mapper = mapper;
            _ordersReposetory = ordersReposetory;
        }
        public async Task<Resulte<ReviewDTO>> AddReviewServise(long orderId, AddReviewDTO review)
        {
            if (orderId != review.OrderId)
            {
                return Resulte<ReviewDTO>.Failure("The id's are diffrent");
            }
            Order? checkIfThereIsExistingOrder = await _ordersReposetory.GetOrderByIdReposetory(orderId);
            if (checkIfThereIsExistingOrder == null)
            {
                return Resulte<ReviewDTO>.Failure("There isn't exist order with that ID");
            }
            Review existingReview = await _reviewsReposetory.GetReviewByOrderIdReposetory(orderId);
            if (existingReview != null)
            {
                return Resulte<ReviewDTO>.Failure("There is already review");
            }
            Review reviewToReposetory = _mapper.Map<Review>(review);
            Review reviewFromReposetory = await _reviewsReposetory.AddReviewReposetory(reviewToReposetory);
            return Resulte<ReviewDTO>.Success(_mapper.Map<ReviewDTO>(reviewFromReposetory));
        }

        public async Task<Resulte<ReviewDTO>> GetReviewByOrderIdServise(long orderId)
        {

            Order? checkIfThereIsExistingOrder = await _ordersReposetory.GetOrderByIdReposetory(orderId);
            if (checkIfThereIsExistingOrder == null)
            {
                return Resulte<ReviewDTO>.Failure("There isn't exist order with that ID");
            }
            Review review = await _reviewsReposetory.GetReviewByOrderIdReposetory(orderId);

            return Resulte<ReviewDTO>.Success(_mapper.Map<ReviewDTO>(review));
        }
        public async Task<Resulte<ReviewDTO>> UpdateReviewServise(long id, ReviewDTO review)
        {

            if (id != review.ReviewId)
            {
                return Resulte<ReviewDTO>.Failure("The id's are diffrent");
            }
           
            Review? checkIfThereIsExistingReview = await _reviewsReposetory.GetByidReviewReposetory(id);
            if (checkIfThereIsExistingReview == null)
            {
                return Resulte<ReviewDTO>.Failure("There isn't exist review with that ID");
            }
            Review reviewToReposetory = _mapper.Map<Review>(review);
            await _reviewsReposetory.UpdateReviewReposetory(id, reviewToReposetory);
            return Resulte<ReviewDTO>.Success(null);
        }
    }
}
