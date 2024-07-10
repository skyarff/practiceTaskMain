using StockService.Models.dto;
using StockService.Models;

namespace StockService.Repository.BillRep
{
    public interface IBillService
    {
        Task<Response> GetAllBillsAsync();
        Task<Response> GetBillsByProviderAndCompanyIdAsync(int? providerId, int? companyId);
        Task<Response> GetBillByIdAsync(int billId);
        Task<Response> CreateBillAsync(BillDto billDto);
        Task<Response> DeleteBillAsync(int billId);
        Task<Response> GetBillsInRangeAsync(BillDto billDto);
        Task<Response> GetBillsFilteredAsync(BillDto billDto);
    }
}
