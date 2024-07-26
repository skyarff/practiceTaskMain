using StockService.Models;
using StockService.Models.dto;
using System.Security.Claims;

namespace StockService.Repository.ProductRep
{
    public interface IProductService
    {
        Task<Response> GetProductsFilteredAsync(ProductDto productDto, ClaimsPrincipal User);
        Task<Response> GetAllProductsAsync();
        Task<Response> GetProductByIdAsync(int productId, ClaimsPrincipal User);
        Task<Response> CreateProductAsync(ProductDto productDto, ClaimsPrincipal User);
        Task<Response> DeleteProductAsync(int productId, ClaimsPrincipal User);
        Task<Response> UpdateProductAsync(ProductDto productDto, ClaimsPrincipal User);
    }
}
