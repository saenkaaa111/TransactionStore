namespace TransactionStore.BusinessLayer.Exceptions
{
    public class CurrencyNotReceivedException : Exception
    {
        public CurrencyNotReceivedException(string message) : base(message) { }
    }
}
