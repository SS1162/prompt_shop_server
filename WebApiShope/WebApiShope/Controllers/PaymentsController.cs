using DTO;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebApiShope.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly PayPalService _payPalService;
        private readonly IOrdersServise _ordersServise;

        public PaymentsController(PayPalService payPalService, IOrdersServise ordersServise)
        {
            _payPalService = payPalService;
            _ordersServise = ordersServise;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreatePaymentDTO dto)
        {
            var result = await _payPalService.CreateVerifiedOrder(dto.ClientAmount, dto.Currency, dto.Products);
            if (!result.IsSuccess)
                return BadRequest(new { error = result.ErrorMessage });
            return Ok(new { id = result.Data });
        }

        [HttpPost("capture/{paypalOrderId}")]
        public async Task<IActionResult> Capture(string paypalOrderId, [FromBody] OrdersDTO order)
        {
            var (success, errorMessage) = await _payPalService.CaptureOrder(paypalOrderId);
            if (!success)
                return BadRequest(new { error = "Payment capture failed", details = errorMessage });

            var result = await _ordersServise.AddOrderServise(order);
            if (!result.IsSuccess)
                return BadRequest(new { error = "Order creation failed", details = result.ErrorMessage });

            return Ok(result.Data);
        }
    }

    public record CreatePaymentDTO(decimal ClientAmount, string Currency, List<AddToCartDTO> Products);
}