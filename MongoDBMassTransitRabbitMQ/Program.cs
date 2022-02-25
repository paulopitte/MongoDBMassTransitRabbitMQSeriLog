using MassTransit;
using MongoDBMassTransitRabbitMQ.Handlers;
using MongoDBMassTransitRabbitMQ.Repository;
using Serilog;
using Serilog.Events;


var builder = WebApplication.CreateBuilder(args);





builder.Services.AddMassTransitHostedService();




Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .CreateLogger();

try
{
    // Add services to the container.
    builder.Services.AddSingleton<IOrderRepository, OrderRepository>();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();


    builder.Services.AddMassTransit(cfg =>
    {
        cfg.AddConsumer<OrderConsumer>();
        cfg.UsingRabbitMq((context, config) =>
        {
            config.ReceiveEndpoint("order_queue", e =>
            {
                e.ConfigureConsumer<OrderConsumer>(context);
            });

            config.Host("127.0.0.1", 5672, "/", h =>
            {

                h.Username("guest");
                h.Password("guest");
            });
        });
    });


    Log.Information("Starting web host");
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}






