using System.Data;
using System.Data.SqlClient;
using TransactionStore.BusinessLayer.Services;
using TransactionStore.BusinessLayer.Services.Interfaces;
using TransactionStore.DataLayer.Repository;

namespace TransactionStore.API
{
    public static class ServiceProviderExtensions
    {
        public static void RegisterTransactionStoreServices(this IServiceCollection services)
        {
            services.AddScoped<ITransactionService, TransactionService>();
        }

        public static void RegisterTransactionStoreRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITransactionRepository, TransactionRepository>();
        }
    }
}
