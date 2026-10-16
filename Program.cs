using MassTransit;
using StockTrackerApp.Data;
using StockTrackerApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
// SA Surajit
// var builder = Host.CreateApplicationBuilder(args);

// builder.Services.AddMassTransit(x =>
// {
//     x.UsingRabbitMq((context, cfg) =>
//     {
//         cfg.Host("localhost", "/", h =>
//         {
//             h.Username("admin");
//             h.Password("admin123");
//         });
//     });
// });
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));;
builder.Services.AddHostedService<StockPollingService>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<StockPriceConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h => { h.Username("guest"); h.Password("guest"); });
        cfg.ReceiveEndpoint("stock-price-processor", e =>
        {
            e.ConfigureConsumer<StockPriceConsumer>(context);
        });
    });
});
// var host = builder.Build();
// await host.RunAsync();
var app = builder.Build();
await app.RunAsync();
// EA Surajit
// var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();
app.MapGet("/", () => "Stock Tracker API is running");
app.Run();
