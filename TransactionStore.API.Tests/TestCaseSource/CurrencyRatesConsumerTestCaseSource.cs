using Marvelous.Contracts.ExchangeModels;
using System.Collections;

namespace TransactionStore.API.Tests.TestCaseSource
{
    public class CurrencyRatesConsumerTestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            var currencyRatesExchangeModel = new CurrencyRatesExchangeModel()
            {
                Rates = new()
                {
                    { "USDRUB", 99.00m },
                    { "USDEUR", 0.91m },
                    { "USDJPY", 121.64m },
                    { "USDCNY", 6.37m },
                    { "USDTRY", 14.82m },
                    { "USDRSD", 106.83m }
                }
            };

            yield return new object[] { currencyRatesExchangeModel };
        }
    }

}


