using Microsoft.EntityFrameworkCore;
using RevenueManagement.Application;
using RevenueManagement.Infrastructure.Persistence;
using RevenueManagement.Infrastructure.Persistence.DbContexts;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConnectPersistenceLayer(builder.Configuration["RevenueManagementDB"]);
builder.Services.ConnectApplicationLayer(builder.Configuration["RabbitMQ_Host"]);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    SeedDb();
}

app.Run();

void SeedDb()
{
    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    using var context = serviceScope.ServiceProvider.GetService<RevenueItemsContext>();
    context?.Database.Migrate();
}
