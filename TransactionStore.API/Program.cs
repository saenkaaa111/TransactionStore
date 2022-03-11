using System.Data;
using System.Data.SqlClient;
using TransactionStore.API;
using TransactionStore.API.Configuration;
using TransactionStore.BuisnessLayer.Configuration;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionEnvironmentVariableName = "CONNECTION_STRING";

var connectionString = builder.Configuration.GetValue<string>(connectionEnvironmentVariableName);
builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.EnableAnnotations(); });

builder.Services.AddAutoMapper(typeof(BusinessMapper).Assembly, typeof(DataMapper).Assembly);

builder.Services.RegisterTransactionStoreServices();
builder.Services.RegisterTransactionStoreRepositories();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();