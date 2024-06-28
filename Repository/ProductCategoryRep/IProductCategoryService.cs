using StockService.Models;
using StockService.Models.dto;

namespace StockService.Repository.ProductCategoryRep
{
    public interface IProductCategoryService
    {
        Task<Response> GetProductCategoriesByCompanyIdAsync(int companyId);
        Task<Response> CreateProductCategoryAsync(ProductCategoryDto productCategoryDto);
        Task<Response> DeleteProductCategoryAsync(int productCategoryId);
    }
}
