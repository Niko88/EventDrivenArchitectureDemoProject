using ExampleStore.Services;
using Polly.Extensions.Http;
using Polly;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("OrderApi", (_, client) =>
    {
        var baseAddress = builder.Configuration["OrderApi_BaseAddress"];
        client.BaseAddress = new Uri(baseAddress);
    })
    .SetHandlerLifetime(TimeSpan.FromMinutes(5))  
    .AddPolicyHandler(_ => HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
    );

builder.Services.AddHttpClient("StockApi", (_, client) =>
    {
        var baseAddress = builder.Configuration["StockApi_BaseAddress"];
        client.BaseAddress = new Uri(baseAddress);
    })
    .SetHandlerLifetime(TimeSpan.FromMinutes(5))  
    .AddPolicyHandler(_ => HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
    );


builder.Services.AddSingleton<OrdersApiService>();
builder.Services.AddSingleton<StockApiService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
