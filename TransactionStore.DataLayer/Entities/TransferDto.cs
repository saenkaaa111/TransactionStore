using Marvelous.Contracts;

namespace TransactionStore.DataLayer.Entities
{
    public class TransferDto
    {
        public decimal Amount { get; set; }
        public decimal ConvertedAmount { get; set; }
        public int AccountIdFrom { get; set; }
        public Currency CurrencyFrom { get; set; }
        public int AccountIdTo { get; set; }
        public Currency CurrencyTo { get; set; }
    }
}
