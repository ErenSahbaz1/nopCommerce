namespace Nop.Plugin.Api.Mobile.Models;

public class ProductSummaryModel
{
public string Name { get; set; } = string.Empty;
public decimal Price { get; set; }
public string PriceFormatted { get; set; } = string.Empty;

public string ImageUrl { get; set; } = string.Empty;
}

public class ProductListResponse
{
    public List<ProductSummaryModel> Products { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}