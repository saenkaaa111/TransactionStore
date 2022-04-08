using Marvelous.Contracts.Enums;

namespace TransactionStore.DataLayer.Entities
{
    public class TransferDto
    {
        public long IdFrom { get; set; }
        public long IdTo { get; set; }
        public DateTime Date { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public decimal ConvertedAmount { get; set; }
        public int AccountIdFrom { get; set; }
        public Currency CurrencyFrom { get; set; }
        public int AccountIdTo { get; set; }
        public Currency CurrencyTo { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is TransferDto dto &&
                   IdFrom == dto.IdFrom &&
                   IdTo == dto.IdTo &&
                   Date == dto.Date &&
                   Type == dto.Type &&
                   Amount == dto.Amount &&
                   ConvertedAmount == dto.ConvertedAmount &&
                   AccountIdFrom == dto.AccountIdFrom &&
                   CurrencyFrom == dto.CurrencyFrom &&
                   AccountIdTo == dto.AccountIdTo &&
                   CurrencyTo == dto.CurrencyTo;
        }
    }
}
