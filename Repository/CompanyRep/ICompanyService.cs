using StockService.Models;
using StockService.Models.dto;
using System.Security.Claims;

namespace StockService.Repository.CompanyRep
{
    public interface ICompanyService
    {
        Task<Response> GetAllCompaniesAsync();
        Task<Response> GetCompanyByIdAsync(int CompanyId, ClaimsPrincipal User);
        Task<Response> CreateCompanyAsync(CompanyDto companyDto);
        Task<Response> UpdateCompanyAsync(CompanyDto companyDto);
        Task<Response> DeleteCompanyAsync(int CompanyId);
        Task<Response> GetCompaniesFilteredAsync(CompanyDto companyDto);
    }
}
