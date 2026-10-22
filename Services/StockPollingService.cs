using MassTransit;
using StockTrackerApp.Contracts; // adjust to wherever your message classes are
using System.Net.Http;
using System.Text.Json;

namespace StockTrackerApp.Services;

public class StockPollingService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<StockPollingService> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly string[] _symbols = { "RELIANCE"};

    // public StockPollingService(IPublishEndpoint publishEndpoint)
    //     => _publishEndpoint = publishEndpoint;


    public StockPollingService(
        IServiceScopeFactory scopeFactory,
        ILogger<StockPollingService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // using var scope = _scopeFactory.CreateScope();

                // var publishEndpoint = scope.ServiceProvider
                //     .GetRequiredService<IPublishEndpoint>();

                // await publishEndpoint.Publish(new StockPriceUpdate(
                //     Symbol: "RELIANCE.NS",
                //     Price: quote.Price,
                //     PreviousPrice: quote.PreviousClose,
                //     Timestamp: DateTime.UtcNow
                // ), stoppingToken);

                // await publishEndpoint.Publish(new StockPriceUpdate
                // ("MSFT",
                //     425.50m,
                //     425.50m,
                //     DateTime.UtcNow
                // ), stoppingToken);
 
            foreach (var symbol in _symbols)
            {
                var priceData = await FetchPriceAsync(symbol, stoppingToken); // call external API
                await _publishEndpoint.Publish(new StockPriceUpdate(
                    symbol, priceData.Price, priceData.ChangePercent, DateTime.UtcNow),
                    stoppingToken);
            }
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
   
                _logger.LogInformation("Published stock price update");
            }
            catch (OperationCanceledException)
            {
                // Expected during application shutdown
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish stock price update");
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
    
    private async Task<(decimal Price, decimal ChangePercent)> FetchPriceAsync(string symbol,CancellationToken stoppingToken)
{
    // It is best practice to share a single HttpClient instance via Dependency Injection 
    // rather than instantiating it locally, but keeping your structure here:
    using var client = new HttpClient
    {
        BaseAddress = new Uri("https://downstox.com/"),
        Timeout = TimeSpan.FromSeconds(20)
    };

    using var response = await client.GetAsync($"api/stocks/{Uri.EscapeDataString(symbol)}");
    var json = await response.Content.ReadAsStringAsync();

    if (!response.IsSuccessStatusCode)
    {
        throw new HttpRequestException($"API returned {(int)response.StatusCode}: {json}");
    }
//     var body = await response.Content.ReadAsStringAsync(stoppingToken);
// _logger.LogInformation("Stock API returned HTTP {StatusCode}. Body: {body}", (int)response.StatusCode, body);
    // Parse the JSON response. 
    // Adjust the property names ("price", "changePercent") to match your API's actual schema.
    using var doc = JsonDocument.Parse(json);
    var root = doc.RootElement;
    _logger.LogInformation("Stock API returned HTTP {StatusCode}. Body: {body}", (int)response.StatusCode, root);
    // decimal price = root.GetProperty("price").GetDecimal();
    // decimal changePercent = root.GetProperty("changePercent").GetDecimal();

    return (10, 10);
}
    // private Task<(decimal Price, decimal ChangePercent)> FetchPriceAsync(string symbol)
    // {
    //     // Call a real API like Alpha Vantage, Finnhub, or IEX Cloud here
    //     throw new NotImplementedException();
    // }
}