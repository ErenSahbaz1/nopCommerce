using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Customers;
using Nop.Plugin.Api.Mobile.Models;
using Nop.Plugin.Api.Mobile.Services;
using Nop.Services.Customers;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Api.Mobile.Controllers;

[ApiController]
[Route("api/mobile/[controller]")]
public class AuthController : BasePluginController
{
    private readonly ICustomerService _customerService;
    private readonly ICustomerRegistrationService _customerRegistrationService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly MobileApiSettings _settings;
    private readonly CustomerSettings _customerSettings;

    public AuthController(
        ICustomerService customerService,
        ICustomerRegistrationService customerRegistrationService,
        IJwtTokenService jwtTokenService,
        MobileApiSettings settings,
        CustomerSettings customerSettings)
    {
        _customerService = customerService;
        _customerRegistrationService = customerRegistrationService;
        _jwtTokenService = jwtTokenService;
        _settings = settings;
        _customerSettings = customerSettings;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.UsernameOrEmail) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(ApiResponse<LoginResponse>.ErrorResult("Username/email and password are required"));
        }

        var loginResult = await _customerRegistrationService.ValidateCustomerAsync(
            request.UsernameOrEmail,
            request.Password);

        if (loginResult != CustomerLoginResults.Successful)
        {
            var errorMessage = loginResult switch
            {
                CustomerLoginResults.CustomerNotExist => "Customer does not exist",
                CustomerLoginResults.Deleted => "Customer has been deleted",
                CustomerLoginResults.NotActive => "Customer is not active",
                CustomerLoginResults.NotRegistered => "Customer is not registered",
                CustomerLoginResults.LockedOut => "Customer is locked out",
                CustomerLoginResults.WrongPassword => "Wrong password",
                _ => "Login failed"
            };

            return Unauthorized(ApiResponse<LoginResponse>.ErrorResult(errorMessage));
        }

        var customer = _customerSettings.UsernamesEnabled
            ? await _customerService.GetCustomerByUsernameAsync(request.UsernameOrEmail)
            : await _customerService.GetCustomerByEmailAsync(request.UsernameOrEmail);

        if (customer == null)
        {
            return Unauthorized(ApiResponse<LoginResponse>.ErrorResult("Customer not found"));
        }

        var token = await _jwtTokenService.GenerateTokenAsync(customer);
        var expiresAt = DateTime.UtcNow.AddMinutes(_settings.JwtExpirationMinutes);

        var response = new LoginResponse
        {
            Success = true,
            Token = token,
            ExpiresAt = expiresAt,
            CustomerId = customer.Id,
            Email = customer.Email
        };

        return Ok(ApiResponse<LoginResponse>.SuccessResult(response));
    }
}
