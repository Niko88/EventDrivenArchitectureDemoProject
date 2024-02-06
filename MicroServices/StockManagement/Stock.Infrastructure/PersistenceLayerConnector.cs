using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stock.Application.Services;
using Stock.Infrastructure.Persistence.DbContexts;
using Stock.Infrastructure.Persistence.Repositories;

namespace Stock.Infrastructure.Persistence
{
    public static class PersistenceLayerConnector
    {
        public static void ConnectPersistenceLayer(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<StockDbContext>(ob =>
                    ob.UseSqlServer(connectionString,
                        so => so.MigrationsHistoryTable("__MigrationsHistory", "StockManagement"))
                , ServiceLifetime.Transient);

            services.AddTransient<IStockRepository, StockRepository>();
        }
    }
}
