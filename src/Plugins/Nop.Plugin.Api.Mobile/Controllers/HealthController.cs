using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Api.Mobile.Models;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Api.Mobile.Controllers;

[ApiController]
[Route("api/mobile/[controller]")]
public class HealthController : BasePluginController
{
    private readonly MobileApiSettings _settings;

    public HealthController(MobileApiSettings settings)
    {
        _settings = settings;
    }

    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok(ApiResponse<object>.SuccessResult(new
        {
            ok = true,
            name = "Mobile API",
            time = DateTime.UtcNow
        }));
    }

    // Temporary debug endpoint - remove after testing!
    [HttpGet("debug-key")]
    public IActionResult DebugKey()
    {
        return Ok(new { 
            apiKeyLength = _settings.ApiKey?.Length ?? 0,
            apiKeyPreview = _settings.ApiKey?.Substring(0, Math.Min(3, _settings.ApiKey?.Length ?? 0)) + "***"
        });
    }
}