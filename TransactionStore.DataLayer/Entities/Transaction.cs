namespace TransactionStore.DataLayer.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TypeTransaction Type { get; set; }
        public decimal Amount { get; set; }
        public Account Account { get; set; }

    }
}
