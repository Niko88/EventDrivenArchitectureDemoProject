using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Repositories;
using Orders.Infrastructure.Persistence.DBContexts;
using Orders.Infrastructure.Persistence.Repositories;

namespace Orders.Infrastructure.Persistence;

public static class PersistenceLayerConnector
{
    public static void ConnectPersistenceLayer(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<OrdersContext>(ob =>
                ob.UseSqlServer(connectionString,
                    so => so.MigrationsHistoryTable("__MigrationsHistory", "Orders"))
            , ServiceLifetime.Transient);

        services.AddTransient<IOrderRepository, OrderRepository>();
    }
}