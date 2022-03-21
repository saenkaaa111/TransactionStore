﻿using Marvelous.Contracts;

namespace TransactionStore.BusinessLayer.Services
{
    public interface ICalculationService
    {
        decimal ConvertCurrency(Currency currencyFrom, Currency currencyTo, decimal amount);
        Task<decimal> GetAccountBalance(List<long> accauntId);
    }
}