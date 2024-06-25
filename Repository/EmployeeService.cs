using Microsoft.EntityFrameworkCore;
using StockService.Models;
using StockService;
using AutoMapper;
using StockService.Models.dto;

namespace StockService.Repository
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
            Response response = new Response();
            var employIsExists = await _db.Employees.AnyAsync(e => e.FullName == employeeDto.FullName);

            response.IsSuccess = false; 
            response.Message = "Пользователь уже существует.";
            if (!employIsExists)
            {
                var employee = _mapper.Map<EmployeeDto, Employee>(employeeDto);
                _db.Employees.Add(employee);
                await _db.SaveChangesAsync();

                response.IsSuccess = true;
                response.Result = employee;
                response.Message = "Пользователь добавлен.";
            }
            return response;
        }

        public Task<Response> DeleteEmployeeAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetAllEmployeesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetEmployeeByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateEmployeeAsync(EmployeeDto employeeDto)
        {
            throw new NotImplementedException();
        }
    }
}
