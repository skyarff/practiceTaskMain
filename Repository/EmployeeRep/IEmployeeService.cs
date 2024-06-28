﻿using StockService.Models;
using StockService.Models;
using StockService.Models.dto;

namespace StockService.Repository.EmployeeRep
{
    public interface IEmployeeService
    {
        Task<Response> GetEmployeesByStockIdAsync(int stockId);
        Task<Response> GetEmployeesByCompanyIdAsync(int companyId);
        Task<Response> GetEmployeeByIdAsync(int id);
        Task<Response> CreateEmployeeAsync(EmployeeDto employeeDto);
        Task<Response> UpdateEmployeeAsync(int id, EmployeeDto employeeDto);
        Task<Response> DeleteEmployeeAsync(int id);
    }
}
