using FinTrustMini.Api.Contracts.Accounts;
using FinTrustMini.Application.Accounts.CreateAccount;
using FinTrustMini.Application.Accounts.GetAccount;
using Microsoft.AspNetCore.Mvc;

namespace FinTrustMini.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AccountsController : ControllerBase
{
    private readonly CreateAccountService _createAccountService;
    private readonly GetAccountService _getAccountService;

    public AccountsController(CreateAccountService createAccountService, GetAccountService getAccountService)
    {
        _createAccountService = createAccountService;
        _getAccountService = getAccountService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAccountApiRequest request, CancellationToken cancellationToken)
    {
        var createAccountRequest = new CreateAccountRequest(
            request.CustomerId,
            request.Iban,
            request.OpeningBalance);

        var result = await _createAccountService.CreateAsync(createAccountRequest, cancellationToken);

        var response = new CreateAccountApiResponse(
            result.AccountId,
            result.CustomerId,
            result.Iban,
            result.Balance);

        return Created($"/api/accounts/{response.AccountId}", response);
    }

    [HttpGet("{accountId:guid}")]
    public async Task<IActionResult> GetById(Guid accountId, CancellationToken cancellationToken)
    {
        var result = await _getAccountService.GetByIdAsync(accountId, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        var response = new GetAccountApiResponse(
            result.AccountId,
            result.CustomerId,
            result.Iban,
            result.Balance,
            result.Status);

        return Ok(response);
    }
}
