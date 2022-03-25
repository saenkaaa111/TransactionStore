namespace TransactionStore.API.Producers
{
    public interface ITransactionProducer
    {
        Task Main(long id);
    }
}