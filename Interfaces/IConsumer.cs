namespace StockTrackerApp.Interfaces;

public interface IConsumer
{
    // Properties that must be implemented
    string ProviderName { get; }

    // Methods that must be implemented
    void SendNotification(string userId, string message);
}