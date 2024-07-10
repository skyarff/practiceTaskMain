using StockService.Models.dto;
using StockService.Models;

namespace StockService.Repository.UpdRep
{
    public interface IUpdService
    {
        Task<Response> GetAllUpdsAsync();
        Task<Response> GetUpdsInRangeAsync(UpdDto updDto);
        Task<Response> GetUpdsByBillIdAsync(int billId);
        Task<Response> GetUpdByIdAsync(int updId);
        Task<Response> CreateUpdAsync(UpdDto updDto);
        Task<Response> DeleteUpdAsync(int updId);
        Task<Response> GetUpdsFilteredAsync(UpdDto updDto);
    }
}
