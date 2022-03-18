
namespace TransactionStore.BusinessLayer.Services
{
    public interface ICurrencyRates
    {
        Dictionary<string, decimal> GetRates();
    }
}