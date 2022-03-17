using System.Data;
using System.Data.SqlClient;
using TransactionStore.API;
using TransactionStore.API.Configuration;
using TransactionStore.BuisnessLayer.Configuration;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionEnvironmentVariableName = "CONNECTION_STRING";
string _logDirectoryVariableName = "LOG_DIRECTORY";

var connectionString = builder.Configuration.GetValue<string>(connectionEnvironmentVariableName);
builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(connectionString));

string logDirectory = builder.Configuration.GetValue<string>(_logDirectoryVariableName);
var config = new ConfigurationBuilder()
           .SetBasePath(logDirectory)
           .AddXmlFile("NLog.config", optional: true, reloadOnChange: true)
           .Build();

builder.Services.RegisterLogger(config);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.EnableAnnotations(); });
builder.Services.AddAutoMapper(typeof(BusinessMapper).Assembly, typeof(DataMapper).Assembly);
builder.Services.RegisterTransactionStoreServices();
builder.Services.RegisterTransactionStoreRepositories();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();