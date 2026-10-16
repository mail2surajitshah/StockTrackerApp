

namespace StockTrackerApp.Contracts;
public record StockPriceUpdate(
    string Symbol,
    decimal Price,
    decimal ChangePercent,
    DateTime Timestamp
);