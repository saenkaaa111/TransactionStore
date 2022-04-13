using Marvelous.Contracts.Enums;
using System.Collections;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.DataLayer.Entities;

namespace TransactionStore.BusinessLayer.Tests.TestCaseSource
{
    public class DepositTestCaseSourse : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            var depositModel = new TransactionModel()
            {
                Type = TransactionType.Deposit,
                Amount = 600,
                AccountId = 6,
                Currency = Currency.RUB
            };


            var depositDto = new TransactionDto()
            {
                Type = TransactionType.Deposit,
                Amount = 600,
                AccountId = 6,
                Currency = Currency.RUB
            };

            long id = 1;           

            yield return new object[] { depositModel, depositDto, id};
        }
    }
}