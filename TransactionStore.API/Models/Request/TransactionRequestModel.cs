﻿namespace TransactionStore.API.Models
{
    public class TransactionRequestModel
    {
        public decimal Amount { get; set; }
        public int AccountId { get; set; }
    }
}
