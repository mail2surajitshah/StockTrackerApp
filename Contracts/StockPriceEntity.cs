
namespace StockTrackerApp.Contracts;
public class StockPriceEntity
{
    public int Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal ChangePercent { get; set; }
    public DateTime Timestamp { get; set; }
}