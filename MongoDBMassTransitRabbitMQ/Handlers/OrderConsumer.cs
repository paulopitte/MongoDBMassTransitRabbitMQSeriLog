using MassTransit;

namespace MongoDBMassTransitRabbitMQ.Handlers
{
    using Domain;
    using Repository;
    using System.Threading.Tasks;

    public class OrderConsumer : IConsumer<Order>
    {
        private readonly ILogger<OrderConsumer> _logger;
        private readonly IOrderRepository _orderRepository;

        public OrderConsumer(ILogger<OrderConsumer> logger, IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public Task Consume(ConsumeContext<Order> context)
        {
            try
            {
                var order = context.Message;

                _logger.LogInformation($"Message received on consumer: {order.OrderId}");

                _orderRepository.Insert(order);

                _logger.LogInformation($"Order received: {order.OrderId}");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on try to consume order", ex);
            }

            return Task.CompletedTask;
        }
    }
}
