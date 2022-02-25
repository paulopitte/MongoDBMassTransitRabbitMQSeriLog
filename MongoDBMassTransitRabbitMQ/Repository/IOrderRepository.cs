namespace MongoDBMassTransitRabbitMQ.Repository
{
    using Domain;

    public interface IOrderRepository
    {
        void Insert(Order order);
    }
}
