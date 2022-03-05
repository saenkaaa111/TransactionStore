using TransactionStore.API.Configuration;
using TransactionStore.BuisnessLayer.Configuration;
using TransactionStore.BusinessLayer.Services;
using TransactionStore.BusinessLayer.Services.Interfaces;
using TransactionStore.DataLayer.Repository;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(BuisnessMapper).Assembly, typeof(DataMapper).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddSwaggerGen(c => { c.EnableAnnotations(); });

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