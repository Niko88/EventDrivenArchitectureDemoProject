using Microsoft.EntityFrameworkCore;
using Stock.Application;
using Stock.Infrastructure.Persistence;
using Stock.Infrastructure.Persistence.DbContexts;
using Stock.Infrastructure.Persistence.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConnectPersistenceLayer(builder.Configuration["StockDB"]);
builder.Services.ConnectApplicationLayer(builder.Configuration["RabbitMQ_Host"]);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    SeedDb();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

void SeedDb()
{
    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    using var context = serviceScope.ServiceProvider.GetService<StockDbContext>();
    context?.Database.Migrate();

    if (!context.InventoryItems.Any())
    {
        context.InventoryItems.Add(new InventoryItem()
        {
            Sku = "S123BK",
            Name = "Shirt Black",
            AvailableQuantity = 10,
            RevenueCode = "Abc123",
            Price = 10m,
            LastUpdatedDate = DateTime.Now,
            CreatedDate = DateTime.Now
        });
        context.SaveChanges();
    }
}
