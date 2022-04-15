using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ResponseModels;
using RestSharp;

namespace TransactionStore.BusinessLayer.Helpers
{
    public interface IRequestHelper
    {
        Task<IdentityResponseModel> SendRequestCheckValidateToken(string url, string path, string jwtToken);
        Task<RestResponse<T>> SendRequestForConfigs<T>(string url, string path, string jwtToken = "null");
    }
}