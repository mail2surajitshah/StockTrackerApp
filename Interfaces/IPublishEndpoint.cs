
namespace StockTrackerApp.Interfaces;
// The interface definition (the contract)
public interface IPublishEndpoint
{
    // Properties that must be implemented
    string ProviderName { get; }

    // Methods that must be implemented
    void SendNotification(string userId, string message);
}