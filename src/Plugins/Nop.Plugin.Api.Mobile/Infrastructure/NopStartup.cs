using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Api.Mobile.Services;

namespace Nop.Plugin.Api.Mobile.Infrastructure;

/// <summary>
/// Startup configuration for Mobile API plugin
/// </summary>
public class NopStartup : INopStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IJwtTokenService, JwtTokenService>();
    }

    public void Configure(IApplicationBuilder application)
    {
    }

    public int Order => 1000;
}
