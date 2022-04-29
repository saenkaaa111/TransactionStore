using Marvelous.Contracts.Enums;
using Marvelous.Contracts.RequestModels;
using Marvelous.Contracts.ResponseModels;
using System.Collections;

namespace TransactionStore.API.Tests.TestCaseSource
{
    public class AddTransaction_Forbidden_TestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            var identityResponseModel = new IdentityResponseModel()
            {
                Id = 1,
                Role = "role",
                IssuerMicroservice = Microservice.MarvelousResource.ToString()
            };
            var transactionRequestModel = new TransactionRequestModel
            {
                AccountId = 1,
                Amount = 100,
                Currency = Currency.RUB,
            };

            yield return new object[] { identityResponseModel, transactionRequestModel };
        }
    }
}
