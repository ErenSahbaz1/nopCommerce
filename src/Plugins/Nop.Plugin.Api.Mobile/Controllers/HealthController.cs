using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Api.Mobile.Models;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Api.Mobile.Controllers;

[ApiController]
[Route("api/mobile/[controller]")]
public class HealthController : BasePluginController
{
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
}
