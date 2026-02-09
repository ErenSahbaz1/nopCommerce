using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Plugins;

namespace Nop.Plugin.Misc.HelloWorld;

/// <summary>
/// This is the main plugin class - it's the entry point for your plugin
/// BasePlugin provides basic functionality that all plugins need
/// IMiscPlugin marks this as a "Miscellaneous" plugin (there are other types like IPaymentMethod, IShippingMethod, etc.)
/// </summary>
public class HelloWorldPlugin : BasePlugin, IMiscPlugin
{
    /// <summary>
    /// This method is called when the plugin is installed
    /// You can add setup logic here (create database tables, add settings, etc.)
    /// The 'async' keyword means this method can perform asynchronous operations
    /// 'Task' is like a promise that the work will complete
    /// </summary>
    public override async Task InstallAsync()
    {
        // Call the base class install method - this is required
        await base.InstallAsync();
    }

    /// <summary>
    /// This method is called when the plugin is uninstalled
    /// You should clean up any changes you made during installation
    /// </summary>
    public override async Task UninstallAsync()
    {
        // Call the base class uninstall method - this is required
        await base.UninstallAsync();
    }

    /// <summary>
    /// This method returns the URL to the plugin's configuration page
    /// Now we have a configuration page, so we return the URL to it
    /// </summary>
    public override string GetConfigurationPageUrl()
    {
        return "/Admin/HelloWorld/Configure";
    }
}
