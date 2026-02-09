using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.HelloWorld.Controllers;

/// <summary>
/// Controller for the Hello World plugin
/// This handles the admin configuration page
/// </summary>
[AuthorizeAdmin] // Only admins can access this
[Area(AreaNames.ADMIN)] // This is in the admin area
public class HelloWorldController : BaseController
{
    /// <summary>
    /// This action shows the configuration page
    /// The URL will be: /Admin/HelloWorld/Configure
    /// </summary>
    public IActionResult Configure()
    {
        // Return a simple view
        return View("~/Plugins/Misc.HelloWorld/Views/Configure.cshtml");
    }
}
