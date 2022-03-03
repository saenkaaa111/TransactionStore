namespace TransactionStore.API.Models
{
    public class TransactionInputModel
    {
        public DateTime Date { get; set; }
        public TypeTransaction Type { get; set; }
        public decimal Amount { get; set; }
        public int AccountId { get; set; }
    }
}
