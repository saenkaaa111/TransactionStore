using Marvelous.Contracts.Enums;
using System.Collections;
using System.Collections.Generic;
using TransactionStore.DataLayer.Entities;

namespace TransactionStore.BusinessLayer.Tests.TestCaseSource
{
    public class GetTransactionsByAccountIdsTestCaseSourse : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            var ids = new List<int>() { 1, 2 };

            var transactions = new List<TransactionDto>()
            {
                new TransactionDto
                {
                    Id = 1,
                    AccountId = 1,
                    Amount = 500,
                    Currency = Currency.RUB,
                    Type = TransactionType.Deposit
                },
                new TransactionDto
                {
                    Id = 2,
                    AccountId = 1,
                    Amount = -200,
                    Currency = Currency.USD,
                    Type = TransactionType.Withdraw
                },
                new TransactionDto
                {
                    Id = 3,
                    AccountId = 1,
                    Amount = 700,
                    Currency = Currency.RUB,
                    Type = TransactionType.Transfer
                },
                new TransactionDto
                {
                    Id = 4,
                    AccountId = 2,
                    Amount = 700,
                    Currency = Currency.USD,
                    Type = TransactionType.Transfer
                }
            };

            var expected = new ArrayList()
            {
                new TransactionDto
                {
                    Id = 1,
                    AccountId = 1,
                    Amount = 500,
                    Currency = Currency.RUB,
                    Type = TransactionType.Deposit
                },
                new TransactionDto
                {
                    Id = 2,
                    AccountId = 1,
                    Amount = -200,
                    Currency = Currency.USD,
                    Type = TransactionType.Withdraw
                },
                new TransferDto
                {
                    AccountIdFrom = 1,
                    AccountIdTo = 2,
                    IdFrom = 3,
                    IdTo = 4,
                    Amount = 700m,
                    ConvertedAmount = 700,
                    CurrencyFrom = Currency.RUB,
                    CurrencyTo = Currency.USD,
                    Type = TransactionType.Transfer
                }
            };

            yield return new object[] { ids, transactions, expected };
        }
    }
}
