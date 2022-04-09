using Marvelous.Contracts.Enums;
using RestSharp;

namespace TransactionStore.BusinessLayer.Helpers
{
    public interface IRequestHelper
    {
        Task<RestResponse> SendRequestCheckValidateToken(string url, string path, string jwtToken);
        Task<RestResponse<T>> SendRequestForConfigs<T>(string url, string path, string jwtToken = "null");
    }
}