using FinTrustMini.Application.Accounts.CreateAccount;
using FinTrustMini.Application.Accounts.GetAccount;
using FinTrustMini.Application.Transfers.CreateTransfer;
using FinTrustMini.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<CreateAccountService>();
builder.Services.AddScoped<GetAccountService>();
builder.Services.AddScoped<CreateTransferService>();
builder.Services.AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
