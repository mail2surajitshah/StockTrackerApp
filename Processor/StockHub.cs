
using Microsoft.AspNetCore.SignalR;

namespace StockTrackerApp.Processor;
public class StockHub : Hub
{
    public async Task JoinSymbolGroup(string symbol)
        => await Groups.AddToGroupAsync(Context.ConnectionId, symbol);
}