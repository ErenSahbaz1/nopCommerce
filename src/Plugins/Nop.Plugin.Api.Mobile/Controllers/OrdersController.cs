using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Api.Mobile.Models;
using Nop.Plugin.Api.Mobile.Services;
using Nop.Services.Catalog;
using Nop.Services.Orders;
using Nop.Core;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Api.Mobile.Controllers;

[ApiController]
[Route("api/mobile/[controller]")]
public class OrdersController : BasePluginController
{
    private readonly IOrderService _orderService;
    private readonly IStoreContext _storeContext;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPriceFormatter _priceFormatter;

    public OrdersController(
        IOrderService orderService,
        IStoreContext storeContext,
        IJwtTokenService jwtTokenService,
        IPriceFormatter priceFormatter)
    {
        _orderService = orderService;
        _storeContext = storeContext;
        _jwtTokenService = jwtTokenService;
        _priceFormatter = priceFormatter;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery] int page = 0, [FromQuery] int pageSize = 20)
    {
        if (pageSize > 100)
            pageSize = 100;

        var customerId = await GetCustomerIdFromTokenAsync();
        if (!customerId.HasValue)
            return Unauthorized(ApiResponse<OrderListResponse>.ErrorResult("Authentication required"));

        var store = await _storeContext.GetCurrentStoreAsync();

        var orders = await _orderService.SearchOrdersAsync(
            storeId: store.Id,
            customerId: customerId.Value,
            pageIndex: page,
            pageSize: pageSize);

        var orderModels = new List<OrderSummaryModel>();
        foreach (var order in orders)
        {
            orderModels.Add(await MapOrderAsync(order));
        }

        var response = new OrderListResponse
        {
            Orders = orderModels,
            TotalCount = orders.TotalCount,
            PageIndex = page,
            PageSize = pageSize,
            TotalPages = orders.TotalPages
        };

        return Ok(ApiResponse<OrderListResponse>.SuccessResult(response));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var customerId = await GetCustomerIdFromTokenAsync();
        if (!customerId.HasValue)
            return Unauthorized(ApiResponse<OrderSummaryModel>.ErrorResult("Authentication required"));

        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
            return NotFound(ApiResponse<OrderSummaryModel>.ErrorResult("Order not found"));

        if (order.CustomerId != customerId.Value)
            return NotFound(ApiResponse<OrderSummaryModel>.ErrorResult("Order not found"));

        var model = await MapOrderAsync(order);
        return Ok(ApiResponse<OrderSummaryModel>.SuccessResult(model));
    }

    private async Task<int?> GetCustomerIdFromTokenAsync()
    {
        var authHeader = Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            return null;

        var token = authHeader.Substring("Bearer ".Length).Trim();
        return await _jwtTokenService.ValidateTokenAsync(token);
    }

    private async Task<OrderSummaryModel> MapOrderAsync(Nop.Core.Domain.Orders.Order order)
    {
        return new OrderSummaryModel
        {
            Id = order.Id,
            OrderNumber = order.CustomOrderNumber ?? order.Id.ToString(),
            CreatedOnUtc = order.CreatedOnUtc,
            OrderTotal = order.OrderTotal,
            OrderTotalFormatted = await _priceFormatter.FormatPriceAsync(order.OrderTotal),
            CurrencyCode = order.CustomerCurrencyCode ?? string.Empty,
            OrderStatus = order.OrderStatus.ToString(),
            PaymentStatus = order.PaymentStatus.ToString(),
            ShippingStatus = order.ShippingStatus.ToString()
        };
    }
}
