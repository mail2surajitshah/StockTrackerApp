using MassTransit;
using Microsoft.AspNetCore.SignalR;
using StockTrackerApp.Contracts;

namespace StockTrackerApp.Processor;
public class StockPriceBroadcastConsumer : IConsumer<StockPriceUpdate>
{
    private readonly IHubContext<StockHub> _hub;
    public StockPriceBroadcastConsumer(IHubContext<StockHub> hub) => _hub = hub;

    public async Task Consume(ConsumeContext<StockPriceUpdate> context)
    {
        await _hub.Clients.Group(context.Message.Symbol)
            .SendAsync("PriceUpdated", context.Message);
    }
}