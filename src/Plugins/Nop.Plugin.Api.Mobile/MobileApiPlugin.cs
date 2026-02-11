using Nop.Core;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;

namespace Nop.Plugin.Api.Mobile;

/// <summary>
/// Mobile API plugin - provides RESTful API endpoints for mobile applications
/// </summary>
public class MobileApiPlugin : BasePlugin, IMiscPlugin
{
    private readonly IWebHelper _webHelper;
    private readonly ISettingService _settingService;
    private readonly ILocalizationService _localizationService;

    public MobileApiPlugin(
        IWebHelper webHelper,
        ISettingService settingService,
        ILocalizationService localizationService)
    {
        _webHelper = webHelper;
        _settingService = settingService;
        _localizationService = localizationService;
    }

    public override async Task InstallAsync()
    {
        var settings = new MobileApiSettings
        {
            JwtSecretKey = GenerateSecretKey(),
            JwtIssuer = "nopCommerce",
            JwtAudience = "nopCommerce-Mobile",
            JwtExpirationMinutes = 1440
        };

        await _settingService.SaveSettingAsync(settings);

        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {
            ["Plugins.Api.Mobile.FriendlyName"] = "Mobile API",
            ["Plugins.Api.Mobile.Description"] = "Mobile API plugin (JWT auth + orders endpoints)",
            ["Plugins.Api.Mobile.Settings.JwtSecretKey"] = "JWT Secret Key",
            ["Plugins.Api.Mobile.Settings.JwtExpirationMinutes"] = "JWT Expiration (minutes)"
        });

        await base.InstallAsync();
    }

    public override async Task UninstallAsync()
    {
        await _settingService.DeleteSettingAsync<MobileApiSettings>();
        await _localizationService.DeleteLocaleResourcesAsync("Plugins.Api.Mobile");
        await base.UninstallAsync();
    }

   public override string GetConfigurationPageUrl()
    {
        return $"{_webHelper.GetStoreLocation()}Admin/MobileApiConfig/Configure";
    }

    private static string GenerateSecretKey()
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
        return new string(Enumerable.Repeat(chars, 64)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
