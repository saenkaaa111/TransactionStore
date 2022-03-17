
namespace TransactionStore.BusinessLayer.Services.Interfaces
{
    public interface ICurrencyRates
    {
        Dictionary<string, decimal> GetRates();
    }
}