using Marvelous.Contracts.Enums;

namespace TransactionStore.DataLayer.Entities
{
    public class TransactionDto
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public int AccountId { get; set; }
        public Currency Currency { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is TransactionDto dto &&
                   Id == dto.Id &&
                   Date == dto.Date &&
                   Type == dto.Type &&
                   Amount == dto.Amount &&
                   AccountId == dto.AccountId &&
                   Currency == dto.Currency;
        }
    }
}
