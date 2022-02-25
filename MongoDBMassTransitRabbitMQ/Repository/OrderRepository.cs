namespace MongoDBMassTransitRabbitMQ.Repository
{
    using Domain;
    using Factory;

    public class OrderRepository : IOrderRepository
    {

        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(ILogger<OrderRepository> logger) =>
            _logger = logger;


        public void Insert(Order order)
        {
            try
            {
                var collection = MongoDBConnectionFactory<Order>.GetCollection();
                    collection.InsertOne(order);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao tentar inserir um novo pedido. OrderId: {order.OrderId}", ex);
                throw;
            }
        }
    }
}
