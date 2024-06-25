using StockService.Models;
using StockService.Models;
using StockService.Models.dto;

namespace StockService.Repository
{
    public interface IEmployeeService
    {


        Task<Response> GetAllEmployeesAsync();
        Task<Response> GetEmployeeByIdAsync(int id);
        Task<Response> CreateEmployeeAsync(EmployeeDto employeeDto);
        Task<Response> UpdateEmployeeAsync(EmployeeDto employeeDto);
        Task<Response> DeleteEmployeeAsync(int id);
    }
}
