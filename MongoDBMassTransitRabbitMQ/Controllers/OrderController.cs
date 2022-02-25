using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace MongoDBMassTransitRabbitMQ.Controllers
{
    using Domain;

    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IBusControl _bus;

        public OrderController(ILogger<OrderController> logger, IBusControl bus)
        {
            _logger = logger;
            _bus = bus;
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order order)
        {
            await _bus.Publish(order);

            _logger.LogInformation($"Message received. OrderId: {order.OrderId}");

            return Ok($"{DateTime.Now:o}");
        }


    }
}
