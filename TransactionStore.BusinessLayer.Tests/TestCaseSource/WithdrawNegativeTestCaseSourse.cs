using Marvelous.Contracts.Enums;
using System.Collections;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.DataLayer.Entities;

namespace TransactionStore.BusinessLayer.Tests.TestCaseSource
{
    public class WithdrawNegativeTestCaseSourse : IEnumerable
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
            

            yield return new object[] { withdraw };
        }
    }
}
