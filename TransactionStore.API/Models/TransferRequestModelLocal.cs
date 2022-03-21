using System.ComponentModel.DataAnnotations;

namespace TransactionStore.API.Models
{
    public class TransferRequestModelLocal
    {
        [Required]
        [Range(1, 99999)]
        public decimal Amount { get; set; }

        [Required]
        public int AccountIdFrom { get; set; }

        [Required]
        [Range(1, 7)]
        public CurrencyLocal CurrencyFrom { get; set; }

        [Required]
        public int AccountIdTo { get; set; }

        [Required]
        [Range(1, 7)]
        public CurrencyLocal CurrencyTo { get; set; }
    }

}
