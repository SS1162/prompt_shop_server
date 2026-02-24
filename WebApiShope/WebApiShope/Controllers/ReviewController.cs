using DTO;
using Microsoft.AspNetCore.Mvc;
using Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShope.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewsServise _reviewsServise;
        public ReviewController(IReviewsServise reviewsServise) 
        { 
            this._reviewsServise = reviewsServise;
        }
        // POST api/<OrdersController>/5/review   
        [HttpPost]
        public async Task<ActionResult<ReviewDTO>> AddReviewAsync( [FromForm] AddReviewDTO dto)
        {
            Resulte<ReviewDTO> respone = await _reviewsServise.AddReviewServise(dto.OrderId, dto);
            if(!respone.IsSuccess)
            {
                return BadRequest(respone.ErrorMessage);
            }
            return CreatedAtAction(nameof(GetReviewByOrderId), new { id = dto.OrderId }, respone.Data);
        }

        // GET api/<OrdersController>/5/review
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDTO>> GetReviewByOrderId([FromBody] long orderId)
        {
            Resulte<ReviewDTO> respone = await _reviewsServise.GetReviewByOrderIdServise(orderId);
            if (!respone.IsSuccess)
            {
                return BadRequest(respone.ErrorMessage);
            }
            return Ok(respone.Data);
        }

        // PUT api/<OrdersController>/5/review
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateReviewAsync(long id,[FromBody] ReviewDTO dto)
        {

            Resulte<ReviewDTO> respone = await _reviewsServise.UpdateReviewServise(id,dto);
            if (!respone.IsSuccess)
            {
                return BadRequest(respone.ErrorMessage);
            }
            return Ok();

        }

        // GET api/<ReviewController>/all
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetAllReviews(int limit, int currentPage)
        {
            Resulte<IEnumerable<ReviewDTO>> response = await _reviewsServise.GetAllReviewsServise(limit, currentPage);
            if (!response.IsSuccess)
            {
                return BadRequest(response.ErrorMessage);
            }
            return Ok(response.Data);
        }
    }
}
