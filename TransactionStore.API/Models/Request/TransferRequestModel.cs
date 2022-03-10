using CurrencyEnum;
namespace TransactionStore.API.Models
{
    public class TransferRequestModel
    {
        public decimal Amount { get; set; }
        public int AccountIdFrom { get; set; }
        public Currency CurrencyFrom { get; set; }
        public int AccountIdTo { get; set; }
        public Currency CurrencyTo { get; set; }
    }
}
