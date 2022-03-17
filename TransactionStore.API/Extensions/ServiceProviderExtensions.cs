using NLog.Extensions.Logging;
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
            services.AddScoped<ICalculationService, CalculationService>();
            services.AddScoped<ICurrencyRates, CurrencyRates>();
        }

        public static void RegisterTransactionStoreRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITransactionRepository, TransactionRepository>();
        }

        public static void RegisterLogger(this IServiceCollection service, IConfiguration config)
        {
            service.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);
            service.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Information);
                loggingBuilder.AddNLog(config);
            });
        }
    }
}
