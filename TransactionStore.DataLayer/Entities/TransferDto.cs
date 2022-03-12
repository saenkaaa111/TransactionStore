using Marvelous.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionStore.DataLayer.Entities
{
    public class TransferDto
    {
        public decimal Amount { get; set; }
        public decimal AmountTo { get; set; }
        public int AccountIdFrom { get; set; }
        public Currency CurrencyFrom { get; set; }
        public int AccountIdTo { get; set; }
        public Currency CurrencyTo { get; set; }
    }
}
