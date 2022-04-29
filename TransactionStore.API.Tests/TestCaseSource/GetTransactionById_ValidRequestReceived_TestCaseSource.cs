using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ResponseModels;
using System.Collections;
using System.Collections.Generic;
using TransactionStore.BusinessLayer.Models;

namespace TransactionStore.API.Tests.TestCaseSource
{
    public class GetTransactionById_ValidRequestReceived_TestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            var identityResponseModel = new IdentityResponseModel()
            {
                Id = 1,
                Role = "role",
                IssuerMicroservice = Microservice.MarvelousCrm.ToString()
            };
            long id = 1;
            var transaction =  new TransactionModel()
                {
                    Id = 1,
                    AccountId = 1,
                    Amount = 500,
                    Currency = Currency.RUB,
                    Type = TransactionType.Deposit
                };

            yield return new object[] { identityResponseModel, id, transaction };
        }
    }
}
