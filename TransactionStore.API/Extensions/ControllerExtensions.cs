using Marvelous.Contracts.Endpoints;
using Microsoft.AspNetCore.Mvc;
using TransactionStore.BusinessLayer.Helpers;

namespace TransactionStore.API.Extensions
{
    public static class ControllerExtensions
    {
        public static async Task ValidateToken(this ControllerBase controller, IRequestHelper requestHelper)
        {
            var token = controller.HttpContext.Request.Headers.Authorization.FirstOrDefault();
            await requestHelper.SendRequestCheckValidateToken("https://piter-education.ru:6042",
                AuthEndpoints.ApiAuth + AuthEndpoints.ValidationMicroservice, token);
        }

    }
}
