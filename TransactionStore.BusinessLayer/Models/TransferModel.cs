using Marvelous.Contracts;

namespace TransactionStore.BusinessLayer.Models
{
    public class TransferModel
    {
        public decimal Amount { get; set; }
        public decimal ConvertedAmount { get; set; }
        public int AccountIdFrom { get; set; }
        public Currency CurrencyFrom { get; set; }
        public int AccountIdTo { get; set; }
        public Currency CurrencyTo { get; set; }
    }
}
