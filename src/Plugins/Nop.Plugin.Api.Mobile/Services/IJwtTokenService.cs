using Nop.Core.Domain.Customers;

namespace Nop.Plugin.Api.Mobile.Services;

/// <summary>
/// JWT token service interface
/// </summary>
public interface IJwtTokenService
{
    Task<string> GenerateTokenAsync(Customer customer);
    Task<int?> ValidateTokenAsync(string token);
}
