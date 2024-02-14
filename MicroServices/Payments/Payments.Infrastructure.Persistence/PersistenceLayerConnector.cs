using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Payments.Application.Repositories;
using Payments.Infrastructure.DbContexts;
using Payments.Infrastructure.Repositories;

namespace Payments.Infrastructure
{
    public static class PersistenceLayerConnector
    {
        public static void ConnectPersistenceLayer(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<PaymentsContext>(ob =>
                    ob.UseSqlServer(connectionString,
                        so => so.MigrationsHistoryTable("__MigrationsHistory", "Payments"))
                , ServiceLifetime.Transient);

            services.AddTransient<IPaymentsRepository, PaymentsRepository>();
        }
    }
}
