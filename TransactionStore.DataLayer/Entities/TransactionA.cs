﻿namespace TransactionStore.DataLayer.Entities
{
    public class TransactionA
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public int AccountId { get; set; }
    }
}
