using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orders.Contracts.Commands;
using Orders.Infrastructure.DBContexts;
using Orders.Infrastructure.Entities;
using Orders.Infrastructure.Repositories;
using Orders.Infrastructure.Sagas.StateMachines;

namespace Orders.Infrastructure
{
    public static class OrchestratorConnector
    {
        public static void ConnectOrchestrator(this IServiceCollection services, string hostName, string connectionString)
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

                //Configure request clients (event publishers)
                cfg.AddRequestClient<InitiateOrder>();

                //Configure EventBus, in our case RabbitMQ
                cfg.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(hostName);
                    configurator.ConfigureEndpoints(context);
                });
            });

            services.AddDbContext<OrdersContext>(ob =>
                    ob.UseSqlServer(connectionString,
                        so => so.MigrationsHistoryTable("__MigrationsHistory", "Orders"))
                , ServiceLifetime.Transient);

            services.AddTransient<IOrderRepository, OrderRepository>();
        }
    }
}
