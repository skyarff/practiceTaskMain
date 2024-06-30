using StockService.Models;
using StockService.Models;
using StockService.Models.dto;

namespace StockService.Repository.ProductRep
{
    public interface IProductService
    {



        //Task<Response> GetProductsByCompanyIdAsync(int companyId);
        //Task<Response> GetProductsByProviderIdAsync(int companyId);


        //Task<Response> GetProductsByStockIdAsync(int? stockId);
        //Task<Response> GetProductsByBillIdAsync(int companyId);
        //Task<Response> GetProductsByUpdIdAsync(int companyId);


        Task<Response> GetProductsFilteredAsync(ProductDto productDto);



        Task<Response> GetAllProductsAsync();

        Task<Response> GetProductByIdAsync(int productId);
        Task<Response> CreateProductAsync(ProductDto productDto);
        Task<Response> DeleteProductAsync(int productId);
        Task<Response> UpdateProductAsync(ProductDto productDto);
        
    }
}
