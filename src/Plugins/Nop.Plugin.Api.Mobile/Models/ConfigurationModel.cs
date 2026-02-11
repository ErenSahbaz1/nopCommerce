using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Api.Mobile.Models;

public record ConfigurationModel : BaseNopModel
{
    [NopResourceDisplayName("Plugins.Api.Mobile.JwtSecretKey")]
    public string JwtSecretKey { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Api.Mobile.JwtIssuer")]
    public string JwtIssuer { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Api.Mobile.JwtAudience")]
    public string JwtAudience { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Api.Mobile.JwtExpirationMinutes")]
    public int JwtExpirationMinutes { get; set; }

    [NopResourceDisplayName("Plugins.Api.Mobile.ApiKey")]
    public string ApiKey { get; set; } = string.Empty;
}