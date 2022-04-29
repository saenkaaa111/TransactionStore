using Marvelous.Contracts.Enums;
using Marvelous.Contracts.RequestModels;
using Marvelous.Contracts.ResponseModels;
using System.Collections;

namespace TransactionStore.API.Tests.TestCaseSource
{
    public class AddTransfer_NotValidModelReceived_TestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            var identityResponseModel = new IdentityResponseModel()
            {
                Id = 1,
                Role = "role",
                IssuerMicroservice = Microservice.MarvelousCrm.ToString()
            };
            var transferRequestModel = new TransferRequestModel
            {
                Amount = 1000000000m,
                AccountIdTo = 0,
                CurrencyFrom = Currency.AFN,
            };

            yield return new object[] { identityResponseModel, transferRequestModel };
        }
    
    }
}
