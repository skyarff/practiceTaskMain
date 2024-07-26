using StockService.Models.dto;
using StockService.Models;
using System.Security.Claims;

namespace StockService.Repository.BillRep
{
    public interface IBillService
    {
        Task<Response> GetAllBillsAsync();
        Task<Response> GetBillsByProviderAndCompanyIdAsync(int? providerId, int? companyId, ClaimsPrincipal User);
        Task<Response> GetBillByIdAsync(int billId, ClaimsPrincipal User);
        Task<Response> CreateBillAsync(BillDto billDto, ClaimsPrincipal User);
        Task<Response> DeleteBillAsync(int billId, ClaimsPrincipal User);
        Task<Response> GetBillsFilteredAsync(BillDto billDto, ClaimsPrincipal User);
    }
}
