using StockService.Models;
using StockService.Models.dto;

namespace StockService.Repository.CompanyRep
{
    public interface ICompanyService
    {
        Task<Response> GetAllCompaniesAsync();
        Task<Response> GetCompanyByIdAsync(int id);
        Task<Response> CreateCompanyAsync(CompanyDto companyDto);
        Task<Response> UpdateCompanyAsync(int id, CompanyDto companyDto);
        Task<Response> DeleteCompanyAsync(int id);
    }
}
