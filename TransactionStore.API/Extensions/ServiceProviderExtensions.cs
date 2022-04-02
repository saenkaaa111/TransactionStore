using MassTransit;
using NLog.Extensions.Logging;
using TransactionStore.API.Consumers;
using TransactionStore.API.Producers;
using TransactionStore.BusinessLayer.Services;
using TransactionStore.DataLayer.Repository;

namespace TransactionStore.API
{
    public static class ServiceProviderExtensions
    {
        public static void AddTransactionStoreServices(this IServiceCollection services)
        {
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ITransactionProducer, TransactionProducer>();
            services.AddScoped<ICalculationService, CalculationService>();
            services.AddSingleton<ICurrencyRatesService, CurrencyRatesService>();
        }

        public static void AddTransactionStoreRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITransactionRepository, TransactionRepository>();
        }

        public static void AddLogger(this IServiceCollection service, IConfiguration config)
        {
            service.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);
            service.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Information);
                loggingBuilder.AddNLog(config);
            });
        }

        public static void AddMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<CurrencyRatesConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ReceiveEndpoint("currencyRatesQueue", e =>
                    {
                        e.ConfigureConsumer<CurrencyRatesConsumer>(context);
                    });
                });
            });
        }
    }
}
