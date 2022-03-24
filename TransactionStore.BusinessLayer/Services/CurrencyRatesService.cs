using Marvelous.Contracts.ExchangeModels;

namespace TransactionStore.BusinessLayer.Services
{
    public class CurrencyRatesService : ICurrencyRatesService
    {
        public CurrencyRatesExchangeModel CurrencyRatesModel { get; set; }

        public Dictionary<string, decimal> SaveCurrencyRates(CurrencyRatesExchangeModel currencyRatesModel)
        {
            CurrencyRatesModel = currencyRatesModel;
            return CurrencyRatesModel.Rates;
        }

        //public Dictionary<Currency, decimal> GetCurrensyRates(Dictionary<string, decimal> keyValuePairs)
        //{

        //    return keyValuePairs.ToDictionary(x => ReplaceKey(x.Key), x => x.Value);
        //}

        //private Currency ReplaceKey(string currencyPair)
        //{
        //    switch (currencyPair)
        //    {
        //        case "USDRUB":
        //            return Currency.RUB;
        //        case "USDEUR":
        //            return Currency.EUR;
        //        case "USDJPY":
        //            return Currency.JPY;
        //        case "USDTRY":
        //            return Currency.TRY;
        //        case "USDRSD":
        //            return Currency.RSD;
        //    }

        //    return Currency.USD;
        //}
    }

}
