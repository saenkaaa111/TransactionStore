namespace TransactionStore.BusinessLayer.Exceptions
{
    public class DbTimeoutException : Exception
    {
        public DbTimeoutException(string message) : base(message) { }
    }
}
