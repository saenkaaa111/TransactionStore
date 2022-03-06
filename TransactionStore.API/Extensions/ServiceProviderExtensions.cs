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
        public static void AddConnectionString(this IServiceCollection services)
        {
            services.AddTransient<IDbConnection>(sp => 
            new SqlConnection("Data Source = 80.78.240.16; Database=Transaction;User Id = student; Password=qwe!23;"));
        }
    }
}
