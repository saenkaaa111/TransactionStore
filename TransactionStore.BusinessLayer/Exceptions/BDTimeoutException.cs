namespace TransactionStore.BusinessLayer.Exceptions
{
    public class BDTimeoutException : Exception
    {
        public BDTimeoutException(string message) : base(message) { }
    }
}
