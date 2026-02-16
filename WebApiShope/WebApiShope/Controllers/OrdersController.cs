using Azure;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShope.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersServise _ordersServise;
        public OrdersController(IOrdersServise ordersServise)
        {
            this._ordersServise = ordersServise;
        }
        // GET: api/<OrdersController>
        [HttpGet("{orderId}/orderItems")]
        async public Task<ActionResult<IEnumerable<OrderItemDTO>>> GetOrdersItems([FromBody] long orderId)
        {
            Resulte<IEnumerable<OrderItemDTO>> reaspone = await _ordersServise.GetOrderItemsServise(orderId);
            if (!reaspone.IsSuccess  )
            {
                return BadRequest(reaspone.ErrorMessage);
            }
            if (reaspone.Data.Any())
            {
                return NoContent(); 
            }
            return Ok(reaspone.Data);
        }
        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FullOrderDTO>> GetByID(long id)
        {
            FullOrderDTO order = await _ordersServise.GetByIdOrderServise(id);
            if (order == null)
            {
                return NoContent();
            }
            return Ok(order);
        }

        // POST api/<OrdersController>
        [HttpPost]
        public async Task<ActionResult<FullOrderDTO>> AddOrder([FromBody] OrdersDTO order)
        {

            Resulte<FullOrderDTO> reaspone =await _ordersServise.AddOrderServise(order);
            if (!reaspone.IsSuccess)
            {
                return BadRequest(reaspone.ErrorMessage);
            }
          
            return CreatedAtAction(nameof(GetByID), new { id = reaspone.Data.OrderID}, reaspone.Data);

        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        async public Task<ActionResult> UpdateStatuse(long id, [FromBody] FullOrderDTO order)
        {
            Resulte<FullOrderDTO> reaspone=await _ordersServise.UpdateStatusServise(id, order);
            if (!reaspone.IsSuccess)
            {
                return BadRequest(reaspone.ErrorMessage);
            }
            return Ok();
        }
    }
}
