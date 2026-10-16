using MassTransit;
using StockTrackerApp.Contracts; // adjust to wherever your message classes are

namespace StockTrackerApp.Services;

public class StockPollingService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<StockPollingService> _logger;

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
                using var scope = _scopeFactory.CreateScope();

                var publishEndpoint = scope.ServiceProvider
                    .GetRequiredService<IPublishEndpoint>();

                await publishEndpoint.Publish(new StockPriceUpdate
                ("MSFT",
                    425.50m,
                    425.50m,
                    DateTime.UtcNow
                ), stoppingToken);

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
}