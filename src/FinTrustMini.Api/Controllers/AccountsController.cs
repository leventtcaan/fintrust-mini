using FinTrustMini.Api.Contracts.Accounts;
using FinTrustMini.Application.Accounts.CreateAccount;
using Microsoft.AspNetCore.Mvc;

namespace FinTrustMini.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AccountsController : ControllerBase
{
    private readonly CreateAccountService _createAccountService;

    public AccountsController(CreateAccountService createAccountService)
    {
        _createAccountService = createAccountService;
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
}
