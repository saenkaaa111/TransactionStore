namespace TransactionStore.API.Models
{
    public class TransactionOutputModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TypeTransaction Type { get; set; }
        public decimal Amount { get; set; }
        public int AccountId { get; set; }
    }
}
