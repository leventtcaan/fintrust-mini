using FinTrustMini.Api.Middleware;
using FinTrustMini.Application.Accounts.CreateAccount;
using FinTrustMini.Application.Accounts.GetAccount;
using FinTrustMini.Application.Transfers.CreateTransfer;
using FinTrustMini.Infrastructure;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation failed.",
                Instance = context.HttpContext.Request.Path
            };

            problemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

            var result = new BadRequestObjectResult(problemDetails);
            result.ContentTypes.Add("application/problem+json");

            return result;
        };
    });
builder.Services.AddScoped<CreateAccountService>();
builder.Services.AddScoped<GetAccountService>();
builder.Services.AddScoped<CreateTransferService>();
builder.Services.AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
