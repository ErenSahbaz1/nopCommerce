using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Api.Mobile.Models;
using Nop.Services.Catalog;
using Nop.Services.Media;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Api.Mobile.Controllers;

[ApiController]
[Route("api/mobile/[controller]")]
public class ProductsController : BasePluginController
{
    private readonly IProductService _productService;
    private readonly IPriceFormatter _priceFormatter;
    private readonly IPictureService _pictureService;
    private readonly IStoreContext _storeContext;
    private readonly MobileApiSettings _settings;

    public ProductsController(
        IProductService productService,
        IPriceFormatter priceFormatter,
        IPictureService pictureService,
        IStoreContext storeContext,
        MobileApiSettings settings)
    {
        _productService = productService;
        _priceFormatter = priceFormatter;
        _pictureService = pictureService;
        _storeContext = storeContext;
        _settings = settings;
    }

    /// <summary>
    /// Get list of products
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] int page = 0, [FromQuery] int pageSize = 10)
    {
        try
        {
            // API Key kontrolü
            if (!ValidateApiKey())
                return Unauthorized(ApiResponse<ProductListResponse>.ErrorResult("Invalid or missing API key"));

            // Limit page size to prevent performance issues
            if (pageSize > 100)
                pageSize = 100;

            var store = await _storeContext.GetCurrentStoreAsync();

            // Search for published products only
            var products = await _productService.SearchProductsAsync(
                storeId: store.Id,
                visibleIndividuallyOnly: true,
                pageIndex: page,
                pageSize: pageSize);

            var productModels = new List<ProductSummaryModel>();
            
            foreach (var product in products)
            {
                // Get product picture
                var pictures = await _pictureService.GetPicturesByProductIdAsync(product.Id, 1);
                var picture = pictures.FirstOrDefault();
                var imageUrl = picture != null 
                    ? (await _pictureService.GetPictureUrlAsync(picture)).Url 
                    : string.Empty;

                productModels.Add(new ProductSummaryModel
                {
                    Name = product.Name,
                    Price = product.Price,
                    PriceFormatted = await _priceFormatter.FormatPriceAsync(product.Price),
                    ImageUrl = imageUrl
                });
            }

            var response = new ProductListResponse
            {
                Products = productModels,
                TotalCount = products.TotalCount,
                PageIndex = page,
                PageSize = pageSize,
                TotalPages = products.TotalPages
            };

            return Ok(ApiResponse<ProductListResponse>.SuccessResult(response));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<ProductListResponse>.ErrorResult(
                "An error occurred while fetching products", 
                new List<string> { ex.Message }));
        }
    }

    /// <summary>
    /// Validate API key from request header
    /// </summary>
    private bool ValidateApiKey()
    {
        var apiKey = Request.Headers["X-Api-Key"].FirstOrDefault();
        return !string.IsNullOrEmpty(apiKey) && apiKey == _settings.ApiKey;
    }
}