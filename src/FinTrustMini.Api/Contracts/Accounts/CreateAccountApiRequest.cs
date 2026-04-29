using System.ComponentModel.DataAnnotations;
using FinTrustMini.Api.Validation;

namespace FinTrustMini.Api.Contracts.Accounts;

public sealed record CreateAccountApiRequest(
    [NotEmptyGuid(ErrorMessage = "Customer id cannot be empty.")]
    Guid CustomerId,

    [Required(ErrorMessage = "IBAN is required.")]
    [StringLength(34, MinimumLength = 10, ErrorMessage = "IBAN length must be between 10 and 34 characters.")]
    string Iban,

    [Range(0, 1_000_000, ErrorMessage = "Opening balance must be between 0 and 1000000.")]
    decimal OpeningBalance);
