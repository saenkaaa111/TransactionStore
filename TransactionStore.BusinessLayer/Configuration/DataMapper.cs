using AutoMapper;
using TransactionStore.BusinessLayer.Models;

namespace TransactionStore.API.Configuration
{
    public class DataMapper : Profile
    {
        public DataMapper()
        {
            CreateMap<TransactionModel, Transaction>().ReverseMap();
        }
    }
}
