using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Api.Mobile.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Api.Mobile.Controllers;

[AuthorizeAdmin]
[Area(AreaNames.ADMIN)]
[AutoValidateAntiforgeryToken]
public class MobileApiConfigController : BasePluginController
{
    private readonly ISettingService _settingService;
    private readonly INotificationService _notificationService;
    private readonly ILocalizationService _localizationService;
    private readonly MobileApiSettings _settings;

    public MobileApiConfigController(
        ISettingService settingService,
        INotificationService notificationService,
        ILocalizationService localizationService,
        MobileApiSettings settings)
    {
        _settingService = settingService;
        _notificationService = notificationService;
        _localizationService = localizationService;
        _settings = settings;
    }

    public IActionResult Configure()
    {
        var model = new ConfigurationModel
        {
            JwtSecretKey = _settings.JwtSecretKey,
            JwtIssuer = _settings.JwtIssuer,
            JwtAudience = _settings.JwtAudience,
            JwtExpirationMinutes = _settings.JwtExpirationMinutes,
            ApiKey = _settings.ApiKey
        };

        return View("~/Plugins/Api.Mobile/Views/Configure.cshtml", model);
    }

    [HttpPost]
    public async Task<IActionResult> Configure(ConfigurationModel model)
    {
        if (!ModelState.IsValid)
            return await Task.FromResult(Configure());

        _settings.JwtSecretKey = model.JwtSecretKey;
        _settings.JwtIssuer = model.JwtIssuer;
        _settings.JwtAudience = model.JwtAudience;
        _settings.JwtExpirationMinutes = model.JwtExpirationMinutes;
        _settings.ApiKey = model.ApiKey;

        await _settingService.SaveSettingAsync(_settings);

        _notificationService.SuccessNotification("Settings saved successfully");

        return Configure();
    }
}