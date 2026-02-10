using Nop.Core.Configuration;

namespace Nop.Plugin.Api.Mobile;

/// <summary>
/// Settings for the Mobile API plugin
/// </summary>
public class MobileApiSettings : ISettings
{
    /// <summary>
    /// Secret key used to sign JWT tokens
    /// </summary>
    public string JwtSecretKey { get; set; } = string.Empty;

    /// <summary>
    /// JWT token issuer
    /// </summary>
    public string JwtIssuer { get; set; } = "nopCommerce";

    /// <summary>
    /// JWT token audience
    /// </summary>
    public string JwtAudience { get; set; } = "nopCommerce-Mobile";

    /// <summary>
    /// JWT token expiration time in minutes
    /// </summary>
    public int JwtExpirationMinutes { get; set; } = 1440; // 24 hours
}
