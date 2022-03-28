using AutoMapper;
using Marvelous.Contracts.RequestModels;
using Marvelous.Contracts.ResponseModels;
using TransactionStore.BusinessLayer.Models;

namespace TransactionStore.API.Configuration
{
    public class BusinessMapper : Profile
    {
        public BusinessMapper()
        {
            CreateMap<TransactionRequestModel, TransactionModel>();
            CreateMap<TransactionModel, TransactionResponseModel>();
            CreateMap<TransferRequestModel, TransferModel>();
        }
    }
}
