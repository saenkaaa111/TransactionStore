namespace TransactionStore.DataLayer.Exceptions
{
    public class TransactionsConflictException : Exception
    {
        public TransactionsConflictException(string message) : base(message) { }
    }
}
