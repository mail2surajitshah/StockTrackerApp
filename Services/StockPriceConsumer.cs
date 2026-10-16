using MassTransit;
using StockTrackerApp.Contracts;
using StockTrackerApp.Data;
using StockTrackerApp.Interfaces;

namespace StockTrackerApp.Services;

public class StockPriceConsumer : IConsumer<StockPriceUpdate>
{
    private readonly AppDbContext _db;

    public StockPriceConsumer(AppDbContext db) => _db = db;

    public async Task Consume(ConsumeContext<StockPriceUpdate> context)
    {
        var msg = context.Message;
        _db.StockPrices.Add(new StockPriceEntity
        {
            Symbol = msg.Symbol,
            Price = msg.Price,
            ChangePercent = msg.ChangePercent,
            Timestamp = msg.Timestamp
        });
        await _db.SaveChangesAsync();
    }
}
