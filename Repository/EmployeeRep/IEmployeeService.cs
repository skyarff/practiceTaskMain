using StockService.Models;
using StockService.Models;
using StockService.Models.dto;

namespace StockService.Repository.EmployeeRep
{
    public interface IEmployeeService
    {
        Task<Response> ChangeEmployeePassword(EmployeeDto employeeDto);
        Task<Response> GetAllEmployeesAsync();
        Task<Response> GetEmployeesByStockIdAsync(int? stockId);
        Task<Response> GetEmployeesByCompanyIdAsync(int? companyId);
        Task<Response> GetEmployeeByIdAsync(int employeeId);
        Task<Response> CreateEmployeeAsync(EmployeeDto employeeDto);
        Task<Response> UpdateEmployeeAsync(EmployeeDto employeeDto);
        Task<Response> DeleteEmployeeAsync(int employeeId);
        Task<Response> GetEmployeesFilteredAsync(EmployeeDto employeeDto);
    }
}
