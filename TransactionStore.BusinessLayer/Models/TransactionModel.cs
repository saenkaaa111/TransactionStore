using TransactionStore.DataLayer.Entities;

namespace TransactionStore.BusinessLayer.Models
{
    public class TransactionModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TypeTransaction Type { get; set; }
        public decimal Amount { get; set; }
        public int AccountId { get; set; }
    }
}
