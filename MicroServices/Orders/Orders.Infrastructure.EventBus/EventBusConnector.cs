

using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Consumers;
using Orders.Contracts.Commands;
using Orders.Contracts.Queries;
using Orders.Infrastructure.Persistence.DBContexts;
using Orders.Infrastructure.Persistence.Entities;
using Orders.Infrastructure.Persistence.Sagas.StateMachines;

namespace Orders.Infrastructure.EventBus
{
    public static class EventBusConnector
    {
        public static void ConnectEventBusWithOrchestrators(this IServiceCollection services, string hostName, string connectionString)
        {
            services.AddMassTransit(cfg =>
            {
                //This is just to make the queue naming more visually appealing on RabbitMQ
                cfg.SetKebabCaseEndpointNameFormatter();

                //Configure State Machine (In our case keeping track of the state of Order)
                cfg.AddSagaStateMachine<OrderStateMachine, Order>()
                    .EntityFrameworkRepository(r =>
                    {
                        r.AddDbContext<DbContext,OrdersContext>((_, dbContextOptionsBuilder) =>
                        {
                            dbContextOptionsBuilder.UseSqlServer(connectionString);
                        });
                    });

                //Configure consumers (event handlers)
                cfg.AddConsumersFromNamespaceContaining<GetOrderStatusConsumer>();

                //Configure request clients (event publishers)
                cfg.AddRequestClient<InitiateOrderCommand>();
                cfg.AddRequestClient<GetOrderStatusQuery>();

                //Configure EventBus, in our case RabbitMQ
                cfg.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(hostName);
                    configurator.ConfigureEndpoints(context);
                });
            });


        }
    }
}
