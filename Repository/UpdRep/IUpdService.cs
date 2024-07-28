using StockService.Models.dto;
using StockService.Models;
using System.Security.Claims;

namespace StockService.Repository.UpdRep
{
    public interface IUpdService
    {
        Task<Response> GetAllUpdsAsync();
        Task<Response> GetUpdsByBillIdAsync(int billId, ClaimsPrincipal User);
        Task<Response> GetUpdsByCompanyIdAsync(int companyId, ClaimsPrincipal User);
        Task<Response> GetUpdByIdAsync(int updId, ClaimsPrincipal User);
        Task<Response> CreateUpdAsync(UpdDto updDto, ClaimsPrincipal User);
        Task<Response> DeleteUpdAsync(int updId, ClaimsPrincipal User);
        Task<Response> GetUpdsFilteredAsync(UpdDto updDto, ClaimsPrincipal User);
    }
}
