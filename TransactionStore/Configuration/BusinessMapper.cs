using AutoMapper;
using TransactionStore.API.Models;

namespace TransactionStore.API.Configuration
{
    public class BuisnessMapper : Profile
    {
        public BuisnessMapper()
        {
            CreateMap<TransactionInputModel, TransactionModel>();
            CreateMap<TransactionModel, TransactionOutputModel>();
        }
    }
}
