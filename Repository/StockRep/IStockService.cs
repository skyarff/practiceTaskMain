using StockService.Models;
using StockService.Models.dto;
using System.Security.Claims;

namespace StockService.Repository.StockRep
{
    public interface IStockService
    {
        Task<Response> GetAllStocksAsync();
        Task<Response> GetStocksByCompanyIdAsync(int? companyId, ClaimsPrincipal User);
        Task<Response> GetStockByIdAsync(int stockId, ClaimsPrincipal User);
        Task<Response> CreateStockAsync(StockDto stockDto, ClaimsPrincipal User);
        Task<Response> UpdateEmployeeAsync(StockDto stockDto, ClaimsPrincipal User);
        Task<Response> DeleteStockAsync(int stockId, ClaimsPrincipal User);
        Task<Response> GetStocksFilteredAsync(StockDto stockDto, ClaimsPrincipal User);
    }
}
