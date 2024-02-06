using Microsoft.EntityFrameworkCore;
using Payments.Application;
using Payments.Infrastructure;
using Payments.Infrastructure.DbContexts;
using Payments.Infrastructure.SquirrelPay;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConnectApplicationLayer(builder.Configuration["RabbitMQ_Host"]);
builder.Services.ConnectPersistenceLayer(builder.Configuration["PaymentsDB"]);
builder.Services.ConnectSquirrelPay(new ProviderConfiguration
{
    BaseAddress =   builder.Configuration["PaymentProvider_baseAddress"],
    NotificationUrl = builder.Configuration["NotificationUrl"],
    CreateSessionEndpoint = "CreateSession",
    ClientName = "squirrelPay"
});


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
    using var context = serviceScope.ServiceProvider.GetService<PaymentsContext>();
    context?.Database.Migrate();
}