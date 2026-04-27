using FinTrustMini.Api.Contracts.Transfers;
using FinTrustMini.Application.Transfers.CreateTransfer;
using Microsoft.AspNetCore.Mvc;

namespace FinTrustMini.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TransfersController : ControllerBase
{
    private readonly CreateTransferService _createTransferService;

    public TransfersController(CreateTransferService createTransferService)
    {
        _createTransferService = createTransferService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTransferApiRequest request, CancellationToken cancellationToken)
    {
        var createTransferRequest = new CreateTransferRequest(
            request.FromAccountId,
            request.ToAccountId,
            request.Amount,
            request.Description);

        var result = await _createTransferService.CreateAsync(createTransferRequest, cancellationToken);

        var response = new CreateTransferApiResponse(
            result.TransferId,
            result.FromAccountId,
            result.ToAccountId,
            result.Amount,
            result.Status,
            result.FailureReason);

        return Created($"/api/transfers/{response.TransferId}", response);
    }
}
