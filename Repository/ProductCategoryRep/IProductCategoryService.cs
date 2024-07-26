using StockService.Models;
using StockService.Models.dto;
using System.Security.Claims;

namespace StockService.Repository.ProductCategoryRep
{
    public interface IProductCategoryService
    {
        Task<Response> GetProductCategoriesByCompanyIdAsync(int companyId, ClaimsPrincipal User);
        Task<Response> GetAllProductCategoriesAsync();
        Task<Response> CreateProductCategoryAsync(ProductCategoryDto productCategoryDto, ClaimsPrincipal User);
        Task<Response> DeleteProductCategoryAsync(int productCategoryId, ClaimsPrincipal User);
        Task<Response> GetCategoriesFilteredAsync(ProductCategoryDto productCategoryDto, ClaimsPrincipal User);
    }
}
