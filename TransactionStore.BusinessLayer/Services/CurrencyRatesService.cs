using Marvelous.Contracts.ExchangeModels;

namespace TransactionStore.BusinessLayer.Services
{
    public class CurrencyRatesService : ICurrencyRatesService
    {
        public Dictionary<string, decimal> CurrencyRates { get; set; }

        public void SaveCurrencyRates(CurrencyRatesExchangeModel currencyRatesModel)
        {
            CurrencyRates = currencyRatesModel.Rates;
        }

        //if (currencyRates is null)
        //{
        //    //currencyRates = _cache.Get<Dictionary<string, decimal>>(Key);

        //    string jsonread = File.ReadAllText("dictionary.json");
        //    currencyRates = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(jsonread);
        //}
        //else
        //{
        //    //_cache.Set(Key, currencyRates);

        //    string json = JsonConvert.SerializeObject(currencyRates, Formatting.Indented);
        //    File.WriteAllText("dictionary.json", json);
        //}
    }
}
