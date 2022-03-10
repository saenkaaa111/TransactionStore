using Marvelous.Contracts;

namespace TransactionStore.BusinessLayer.Models
{
    public class TransactionModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public int AccountId { get; set; }
        public Currency Currency { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is TransactionModel model &&
                   Id == model.Id &&
                   Date == model.Date &&
                   Type == model.Type &&
                   Amount == model.Amount &&
                   AccountId == model.AccountId &&
                   Currency == model.Currency;
        }
    }
}
