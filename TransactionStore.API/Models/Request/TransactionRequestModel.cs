using TransactionStore.DataLayer.Entities;

namespace TransactionStore.API.Models
{
    public class TransactionRequestModel
    {
        public DateTime Date { get; set; }
        public TypeTransaction Type { get; set; }
        public decimal Amount { get; set; }
        public int AccountId { get; set; }
    }
}
