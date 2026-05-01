using FinTrustMini.Application.Transfers.AssessTransferRisk;
using FinTrustMini.Application.Transfers.Risk;

namespace FinTrustMini.Application.Tests.Transfers.AssessTransferRisk;

public sealed class AssessTransferRiskServiceTests
{
    [Fact]
    public void Assess_ShouldAllowLowRiskTransfer_WhenNoRiskSignalExists()
    {
        var service = new AssessTransferRiskService(new DefaultTransferRiskPolicy());
        var request = new AssessTransferRiskRequest(
            Guid.NewGuid(),
            Guid.NewGuid(),
            250m,
            "Invoice payment");

        var result = service.Assess(request);

        Assert.True(result.IsAllowed);
        Assert.Equal("Low", result.RiskLevel);
        Assert.Equal(0, result.RiskScore);
        Assert.Contains("No material risk signal detected.", result.Reasons);
    }

    [Fact]
    public void Assess_ShouldRejectHighRiskTransfer_WhenAmountExceedsLimit()
    {
        var service = new AssessTransferRiskService(new DefaultTransferRiskPolicy());
        var request = new AssessTransferRiskRequest(
            Guid.NewGuid(),
            Guid.NewGuid(),
            10_001m,
            "High value payment");

        var result = service.Assess(request);

        Assert.False(result.IsAllowed);
        Assert.Equal("High", result.RiskLevel);
        Assert.Equal(100, result.RiskScore);
        Assert.Contains("Transfer amount exceeds the single transfer limit of 10000.", result.Reasons);
    }

    [Fact]
    public void Assess_ShouldReturnMediumRisk_WhenAmountIsHighButAllowed()
    {
        var service = new AssessTransferRiskService(new DefaultTransferRiskPolicy());
        var request = new AssessTransferRiskRequest(
            Guid.NewGuid(),
            Guid.NewGuid(),
            5_000m,
            "Supplier payment");

        var result = service.Assess(request);

        Assert.True(result.IsAllowed);
        Assert.Equal("Medium", result.RiskLevel);
        Assert.Equal(45, result.RiskScore);
        Assert.Contains("Transfer amount is high and should be monitored.", result.Reasons);
    }
}
