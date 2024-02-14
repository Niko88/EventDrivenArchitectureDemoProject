using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RevenueManagement.Application.Services;
using RevenueManagement.Infrastructure.Persistence.DbContexts;
using RevenueManagement.Infrastructure.Persistence.Repositories;

namespace RevenueManagement.Infrastructure.Persistence
{
    public static class PersistenceLayerConnector
    {
        public static void ConnectPersistenceLayer(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<RevenueItemsContext>(ob =>
                    ob.UseSqlServer(connectionString,
                        so => so.MigrationsHistoryTable("__MigrationsHistory", "RevenueManagement"))
                , ServiceLifetime.Transient);

            services.AddTransient<IRevenueItemsRepository, RevenueItemsRepository>();
        }
    }
}
