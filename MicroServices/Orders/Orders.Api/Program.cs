using Microsoft.EntityFrameworkCore;
using Orders.Infrastructure;
using Orders.Infrastructure.DBContexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConnectOrchestrator(
    hostName: builder.Configuration["RabbitMQ_Host"],
    connectionString: builder.Configuration["OrdersDB"]
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    SeedDb();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void SeedDb()
{
    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    using var context = serviceScope.ServiceProvider.GetService<OrdersContext>();
    context?.Database.Migrate();
}