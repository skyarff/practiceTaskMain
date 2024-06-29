using StockService.Models;
using StockService.Models.dto;

namespace StockService.Repository.CompanyRep
{
    public interface ICompanyService
    {
        Task<Response> GetAllCompaniesAsync();
        Task<Response> GetCompanyByIdAsync(int CompanyId);
        Task<Response> CreateCompanyAsync(CompanyDto companyDto);
        Task<Response> UpdateCompanyAsync(CompanyDto companyDto);
        Task<Response> DeleteCompanyAsync(int CompanyId);
    }
}
