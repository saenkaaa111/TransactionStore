using Marvelous.Contracts.Enums;
using TransactionStore.BusinessLayer.Exceptions;

namespace TransactionStore.BusinessLayer.Helpers
{
    public class CheckCurrencyHelper
    {
        private static readonly List<Currency> _currencyList = new()
        {
            Currency.RUB,
            Currency.EUR,
            Currency.USD,
            Currency.JPY,
            Currency.CNY,
            Currency.RSD,
            Currency.TRY
        };

        public bool CheckCurrency(Currency currency)
        {
            if (_currencyList.Contains(currency))
                return true;

            else
                throw new CurrencyNotReceivedException("The request for the currency value was not received");
        }
    }
}
