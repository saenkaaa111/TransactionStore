﻿using Marvelous.Contracts;

namespace TransactionStore.DataLayer.Entities
{
    public class TransferDto
    {
        public int IdFrom { get; set; }
        public int IdTo { get; set; }
        public DateTime Date { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public decimal ConvertedAmount { get; set; }
        public int AccountIdFrom { get; set; }
        public Currency CurrencyFrom { get; set; }
        public int AccountIdTo { get; set; }
        public Currency CurrencyTo { get; set; }
    }
}
