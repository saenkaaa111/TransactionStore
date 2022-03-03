using AutoMapper;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.DataLayer.Entities;

namespace TransactionStore.API.Configuration
{
    public class DataMapper : Profile
    {
        public DataMapper()
        {
            CreateMap<TransactionModel, TransactionA>().ReverseMap();
        }
    }
}
