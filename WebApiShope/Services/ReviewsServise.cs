using AutoMapper;
using DTO;
using Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Repositories;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
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
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;


        public ReviewsServise(IReviewsReposetory reviewsReposetory, IMapper mapper, IOrdersReposetory ordersReposetory, IConfiguration config, IWebHostEnvironment env)
        {
            this._reviewsReposetory = reviewsReposetory;
            this._mapper = mapper;
            _ordersReposetory = ordersReposetory;
            _config = config;
            _env = env;
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

            if (review.ReviewImg != null)
            {
                using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(review.ReviewImg.OpenReadStream()))
                {
                    ResizeOptions options = new ResizeOptions
                    {
                        Size = new Size(800, 0)
                    };
                    image.Mutate(processor => processor.Resize(options));

                    JpegEncoder encoder = new JpegEncoder
                    {
                        Quality = 75
                    };

                    string physicalPath = Path.Combine(_env.WebRootPath, "reviews", review.ReviewImg.FileName+ ".jpeg");
                    await image.SaveAsync(physicalPath, encoder);
                }
                reviewToReposetory.ReviewImg = review.ReviewImg.FileName;
            }

            Review reviewFromReposetory = await _reviewsReposetory.AddReviewReposetory(reviewToReposetory);

            checkIfThereIsExistingOrder.ReviewId = reviewFromReposetory.ReviewId;
            await _ordersReposetory.UpdateStatusReposetory(checkIfThereIsExistingOrder.OrderId, checkIfThereIsExistingOrder);

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

        public async Task<Resulte<IEnumerable<ReviewDTO>>> GetAllReviewsServise(int limit, int currentPage)
        {


            IEnumerable<Review> reviews = await _reviewsReposetory.GetAllReviewsReposetory(limit, currentPage);
            IEnumerable<ReviewDTO> reviewDTOs = _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
            return Resulte<IEnumerable<ReviewDTO>>.Success(reviewDTOs);

        }



    }
}
