using Marvelous.Contracts.Enums;
using System.Collections;
using TransactionStore.BusinessLayer.Models;

namespace TransactionStore.BusinessLayer.Tests.TestCaseSource
{
    public class WithdrawTestCaseSourse : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            var withdraw = new TransactionModel()
            {
                Id = 1,
                Amount = 100,
                AccountId = 1,
                Currency = Currency.RUB
            };

            long id = 1;

            decimal balance = 300m;

            yield return new object[] { withdraw, id, balance };
        }
    }
}
