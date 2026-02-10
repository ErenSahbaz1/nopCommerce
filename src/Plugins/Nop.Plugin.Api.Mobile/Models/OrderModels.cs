namespace Nop.Plugin.Api.Mobile.Models;

public class OrderSummaryModel
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime CreatedOnUtc { get; set; }
    public decimal OrderTotal { get; set; }
    public string OrderTotalFormatted { get; set; } = string.Empty;
    public string CurrencyCode { get; set; } = string.Empty;
    public string OrderStatus { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public string ShippingStatus { get; set; } = string.Empty;
}

public class OrderListResponse
{
    public List<OrderSummaryModel> Orders { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}
