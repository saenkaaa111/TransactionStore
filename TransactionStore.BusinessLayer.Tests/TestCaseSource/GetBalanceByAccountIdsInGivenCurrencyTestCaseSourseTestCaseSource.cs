using Marvelous.Contracts.Enums;
using System.Collections;
using System.Collections.Generic;
using TransactionStore.DataLayer.Entities;

namespace TransactionStore.BusinessLayer.Tests.TestCaseSource
{
    public class GetBalanceByAccountIdsInGivenCurrencyTestCaseSourseTestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            var expected = 50m;

            var ids = new List<int> { 1 };

            var transactions = new List<TransactionDto>()
            {
                new TransactionDto
                {
                    Id = 1,
                    AccountId = 1,
                    Amount = 100m,
                    Currency = Currency.RUB,
                    Type = TransactionType.Deposit
                },
                new TransactionDto
                {
                    Id = 2,
                    AccountId = 1,
                    Amount = 20m,
                    Currency = Currency.RUB,
                    Type = TransactionType.Withdraw
                },
                new TransactionDto
                {
                    Id = 3,
                    AccountId = 1,
                    Amount = 20m,
                    Currency = Currency.RUB,
                    Type = TransactionType.Transfer
                },
                new TransactionDto
                {
                    Id = 4,
                    AccountId = 1,
                    Amount = 10m,
                    Currency = Currency.RUB,
                    Type = TransactionType.ServicePayment
                }
            };

            yield return new object[] { expected, ids, transactions };
        }
    }
}

