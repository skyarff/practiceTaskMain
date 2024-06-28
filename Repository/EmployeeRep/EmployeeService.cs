using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockService.Models;
using StockService.Models.dto;

namespace StockService.Repository.EmployeeRep
{
    public class EmployeeService : IEmployeeService
    {
        private readonly StockContext _db;
        private readonly IMapper _mapper;
        private Response _response;
        public EmployeeService(IMapper mapper, StockContext db)
        {
            _db = db;
            _mapper = mapper;
            _response = new Response();
        }

        public async Task<Response> CreateEmployeeAsync(EmployeeDto employeeDto)
        {
            var employeeIsExists = await _db.Employees.AnyAsync(e => e.Login == employeeDto.Login);

            _response.IsSuccess = false;
            _response.Message = "Пользователь уже существует.";
            if (!employeeIsExists)
            {
                var employee = _mapper.Map<EmployeeDto, Employee>(employeeDto);
                _db.Employees.Add(employee);
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Result = employee;
                _response.Message = "Пользователь добавлен.";
            }

            return _response;
        }

        public async Task<Response> DeleteEmployeeAsync(int id)
        {
            var employee = await _db.Employees.FindAsync(id);

            _response.IsSuccess = true;
            _response.Message = "Сотрудник не найден.";

            if (employee != null)
            {
                _response.IsSuccess = false;
                _response.Message = "Сотрудник успешно удален.";

                _db.Employees.Remove(employee);
                await _db.SaveChangesAsync();
            }

            return _response;
        }

        public async Task<Response> GetEmployeesByStockIdAsync(int stockId)
        {
            _response.IsSuccess = false;
            _response.Message = "Сотрудники не найдены.";

            var employees = await _db.Employees
                    .Where(e => e.StockId == stockId)
                    .ToListAsync();

            if (employees.Any())
            {
                _response.IsSuccess = true;
                _response.Result = employees;
                _response.Message = $"Сотрудники для склада с ID {stockId} успешно получены.";
            }

            return _response;
        }

        public async Task<Response> GetEmployeesByCompanyIdAsync(int companyId)
        {
            _response.IsSuccess = false;
            _response.Message = "Сотрудники не найдены для указанной компании.";

            var employees = _db.Companies
                .Where(c => c.CompanyId == companyId)
                .SelectMany(c => c.Stocks.SelectMany(w => w.Employees))
                .ToList();

            if (employees.Any())
            {
                _response.IsSuccess = true;
                _response.Result = employees;
                _response.Message = $"Сотрудники для компании с ID {companyId} успешно получены.";
            }

            return _response;
        }

        public async Task<Response> GetEmployeeByIdAsync(int id)
        {
            var employee = await _db.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id);

            _response.IsSuccess = false;
            _response.Message = "Сотрудник не найден.";
            if (employee != null)
            {
                _response.IsSuccess = true;
                _response.Result = employee;
                _response.Message = "Сотрудник найден.";

            }

            return _response;
        }

        public async Task<Response> UpdateEmployeeAsync(int id, EmployeeDto employeeDto)
        {
            var employee = await _db.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id);

            _response.IsSuccess = false;
            _response.Message = "Сотрудник не найден.";

            if (employee != null)
            {
                if (!string.IsNullOrEmpty(employeeDto.FullName))
                    employee.FullName = employeeDto.FullName;

                if (!string.IsNullOrEmpty(employeeDto.JobTitle))
                    employee.JobTitle = employeeDto.JobTitle;

                if (!string.IsNullOrEmpty(employeeDto.Password))
                    employee.Password = employeeDto.Password;

                if (!string.IsNullOrEmpty(employeeDto.Photo))
                    employee.Photo = employeeDto.Photo;

                if (!string.IsNullOrEmpty(employeeDto.Email))
                    employee.Email = employeeDto.Email;

                if (!string.IsNullOrEmpty(employeeDto.Phone))
                    employee.Phone = employeeDto.Phone;


                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Result = employee;
                _response.Message = "Данные сотрудника обновлены.";
            }

            return _response;
        }
    }
}
