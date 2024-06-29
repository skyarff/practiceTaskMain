using StockService.Models.dto;
using StockService.Models;

namespace StockService.Repository.UpdRep
{
    public interface IUpdService
    {
        Task<Response> GetAllUpdsAsync();
        Task<Response> GetUpdsInRangeAsync(UpdDto updDto);
        Task<Response> GetUpdsByProviderIdAsync(int providerId);
        Task<Response> GetUpdByIdAsync(int updId);
        Task<Response> CreateUpdAsync(UpdDto updDto);
        Task<Response> DeleteUpdAsync(int updId);
    }
}
