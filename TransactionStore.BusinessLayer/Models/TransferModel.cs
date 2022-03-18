using Marvelous.Contracts;

namespace TransactionStore.BusinessLayer.Models
{
    public class TransferModel
    {
        public decimal Amount { get; set; }
        public decimal ConvertedAmount { get; set; }
        public int AccountIdFrom { get; set; }
        public string CurrencyFrom { get; set; }
        public int AccountIdTo { get; set; }
        public string CurrencyTo { get; set; }
    }
}
