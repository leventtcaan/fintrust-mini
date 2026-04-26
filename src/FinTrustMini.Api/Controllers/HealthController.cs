using Microsoft.AspNetCore.Mvc;

namespace FinTrustMini.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            Status = "Healthy",
            Service = "FinTrust Mini API",
            CheckedAtUtc = DateTime.UtcNow
        });
    }
}
