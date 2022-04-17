using Marvelous.Contracts.Enums;
using Marvelous.Contracts.RequestModels;
using Marvelous.Contracts.ResponseModels;
using System.Collections;
using System.Collections.Generic;

namespace TransactionStore.API.Tests.TestCaseSource
{
    internal class AddTransfer_Forbidden_TestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            var identityResponseModel = new IdentityResponseModel()
            {
                Id = 1,
                Role = "role",
                IssuerMicroservice = Microservice.MarvelousResource.ToString()
            };
            var transferRequestModel = new TransferRequestModel
            {
                Amount = 100m,
                AccountIdFrom = 1,
                AccountIdTo = 2,
                CurrencyFrom = Currency.RUB,
                CurrencyTo = Currency.EUR
            };

            yield return new object[] { identityResponseModel, transferRequestModel };
        }
    }
}
