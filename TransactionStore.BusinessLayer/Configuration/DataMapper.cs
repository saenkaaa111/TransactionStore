using AutoMapper;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.DataLayer.Entities;

namespace TransactionStore.BuisnessLayer.Configuration
{
    public class DataMapper : Profile
    {
        public DataMapper()
        {
            CreateMap<TransactionModel, TransactionDto>().ReverseMap();
            CreateMap<TransferModel, TransferDto>().ReverseMap();
        }
    }
}
