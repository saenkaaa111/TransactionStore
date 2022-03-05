using AutoMapper;
using TransactionStore.API.Models;
using TransactionStore.BusinessLayer.Models;

namespace TransactionStore.API.Configuration
{
    public class BuisnessMapper : Profile
    {
        public BuisnessMapper()
        {
            CreateMap<TransactionRequestModel, TransactionModel>();
            CreateMap<TransactionModel, TransactionResponseModel>();
        }
    }
}
