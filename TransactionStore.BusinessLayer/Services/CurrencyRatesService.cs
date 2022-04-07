﻿using Marvelous.Contracts.ExchangeModels;

namespace TransactionStore.BusinessLayer.Services
{
    public class CurrencyRatesService : ICurrencyRatesService
    {
        public Dictionary<string, decimal> Rates { get; set; }

        public void SaveCurrencyRates(CurrencyRatesExchangeModel currencyRatesModel)
        {
            Rates = currencyRatesModel.Rates;
        }
    }
}
