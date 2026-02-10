using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Api.Mobile.Infrastructure;

/// <summary>
/// Route provider for Mobile API endpoints
/// </summary>
public class RouteProvider : IRouteProvider
{
    public int Priority => 0;

    public void RegisterRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapControllerRoute(
            name: "MobileApi",
            pattern: "api/mobile/{controller}/{action}/{id?}"
        );
    }
}
