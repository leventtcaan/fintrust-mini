using System.ComponentModel.DataAnnotations;
using FinTrustMini.Api.Validation;

namespace FinTrustMini.Api.Contracts.Transfers;

public sealed record AssessTransferRiskApiRequest(
    [NotEmptyGuid(ErrorMessage = "Source account id cannot be empty.")]
    Guid FromAccountId,

    [NotEmptyGuid(ErrorMessage = "Destination account id cannot be empty.")]
    Guid ToAccountId,

    [Range(0.01, 1_000_000, ErrorMessage = "Transfer amount must be between 0.01 and 1000000.")]
    decimal Amount,

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Description length must be between 1 and 200 characters.")]
    string Description);
