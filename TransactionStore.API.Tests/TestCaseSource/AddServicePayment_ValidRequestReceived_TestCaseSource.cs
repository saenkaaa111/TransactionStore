using Marvelous.Contracts.Enums;
using Marvelous.Contracts.RequestModels;
using Marvelous.Contracts.ResponseModels;
using System.Collections;

namespace TransactionStore.API.Tests.TestCaseSource
{
    public class AddServicePayment_ValidRequestReceived_TestCaseSource : IEnumerable
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

            //IdentityResponseModel identityResponseModel, TransactionRequestModel transactionRequestModel, decimal transactionId
            decimal transactionId = 1;

            yield return new object[] { identityResponseModel, transactionRequestModel, transactionId };
        }
    }
}
