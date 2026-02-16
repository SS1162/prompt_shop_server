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
        public ReviewController() { }
        // POST api/<OrdersController>/5/review   
        [HttpPost]
        public async Task<ActionResult<ReviewDTO>> AddReviewAsync(long orderId, AddReviewDTO dto)
        {
            Resulte<ReviewDTO> respone = await _reviewsServise.AddReviewServise(orderId, dto);
            if(!respone.IsSuccess)
            {
                return BadRequest(respone.ErrorMessage);
            }
            return CreatedAtAction(nameof(GetReviewByOrderId), new { id = orderId }, respone.Data);
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

     
       
    }
}
