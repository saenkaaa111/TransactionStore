using TransactionStore.BusinessLayer.Models;

namespace TransactionStore.API.Producers
{
    public interface ITransactionProducer
    {
        Task NotifyTransactionAdded(long id);
    }
}