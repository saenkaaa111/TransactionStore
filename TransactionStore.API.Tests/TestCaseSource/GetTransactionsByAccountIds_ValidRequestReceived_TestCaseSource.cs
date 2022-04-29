using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ResponseModels;
using System.Collections;
using System.Collections.Generic;
using TransactionStore.DataLayer.Entities;

namespace TransactionStore.API.Tests.TestCaseSource
{
    public class GetTransactionsByAccountIds_ValidRequestReceived_TestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            var identityResponseModel = new IdentityResponseModel()
            {
                Id = 1,
                Role = "role",
                IssuerMicroservice = Microservice.MarvelousCrm.ToString()
            };
            var ids = new List<int> { 1, 2 };
            var transactions = new ArrayList()
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

            yield return new object[] { identityResponseModel, ids, transactions };
        }
    }
}
