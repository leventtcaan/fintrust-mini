using FinTrustMini.Api.Contracts.Transfers;
using FinTrustMini.Application.Transfers.AssessTransferRisk;
using FinTrustMini.Application.Transfers.CreateTransfer;
using FinTrustMini.Application.Transfers.GetTransfer;
using Microsoft.AspNetCore.Mvc;

namespace FinTrustMini.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TransfersController : ControllerBase
{
    private readonly CreateTransferService _createTransferService;
    private readonly GetTransferService _getTransferService;
    private readonly AssessTransferRiskService _assessTransferRiskService;

    public TransfersController(
        CreateTransferService createTransferService,
        GetTransferService getTransferService,
        AssessTransferRiskService assessTransferRiskService)
    {
        _createTransferService = createTransferService;
        _getTransferService = getTransferService;
        _assessTransferRiskService = assessTransferRiskService;
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

    [HttpPost("risk-assessments")]
    public IActionResult AssessRisk(AssessTransferRiskApiRequest request)
    {
        var assessTransferRiskRequest = new AssessTransferRiskRequest(
            request.FromAccountId,
            request.ToAccountId,
            request.Amount,
            request.Description);

        var result = _assessTransferRiskService.Assess(assessTransferRiskRequest);

        var response = new AssessTransferRiskApiResponse(
            result.IsAllowed,
            result.RiskLevel,
            result.RiskScore,
            result.Reasons);

        return Ok(response);
    }

    [HttpGet("{transferId:guid}")]
    public async Task<IActionResult> GetById(Guid transferId, CancellationToken cancellationToken)
    {
        var result = await _getTransferService.GetByIdAsync(transferId, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        var response = new GetTransferApiResponse(
            result.TransferId,
            result.FromAccountId,
            result.ToAccountId,
            result.Amount,
            result.Description,
            result.Status,
            result.FailureReason,
            result.CreatedAtUtc,
            result.CompletedAtUtc,
            result.FailedAtUtc);

        return Ok(response);
    }
}
