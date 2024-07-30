using StockService.Models;
using StockService.Models;
using StockService.Models.dto;
using System.Security.Claims;

namespace StockService.Repository.EmployeeRep
{
    public interface IEmployeeService
    {
        Task<Response> ChangeEmployeePassword(EmployeeDto employeeDto, ClaimsPrincipal User);
        Task<Response> GetAllEmployeesAsync();
        Task<Response> GetEmployeesByStockIdAsync(int? stockId, ClaimsPrincipal User);
        Task<Response> GetEmployeesByCompanyIdAsync(int? companyId, ClaimsPrincipal User);
        Task<Response> GetEmployeeByIdAsync(int employeeId, ClaimsPrincipal User);
        Task<Response> CreateEmployeeAsync(EmployeeDto employeeDto, ClaimsPrincipal User);
        Task<Response> UpdateEmployeeAsync(EmployeeDto employeeDto, ClaimsPrincipal User);
        Task<Response> DeleteEmployeeAsync(int employeeId, ClaimsPrincipal User);
        Task<Response> GetEmployeesFilteredAsync(EmployeeDto employeeDto, ClaimsPrincipal User);
        Task<Response> SignInAsync(EmployeeDto employeeDto);
        Task<Response> Logout();
        Task<Response> GetEmployeeInfo(ClaimsPrincipal User);
        Task<Response> GetNewTokenPairAsync(TokenPair tokenPair);
    }
}
