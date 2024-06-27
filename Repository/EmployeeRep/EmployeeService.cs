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
            Response response = new Response();
            var employeeIsExists = await _db.Employees.AnyAsync(e => e.FullName == employeeDto.FullName);

            response.IsSuccess = false;
            response.Message = "Пользователь уже существует.";
            if (!employeeIsExists)
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

        public async Task<Response> DeleteEmployeeAsync(int id)
        {
            Response response = new Response();
            var employee = await _db.Employees.FindAsync(id);

            response.IsSuccess = true;
            response.Message = "Сотрудник не найден.";

            if (employee != null)
            {
                response.IsSuccess = false;
                response.Message = "Сотрудник успешно удален.";

                _db.Employees.Remove(employee);
                await _db.SaveChangesAsync();
            }

            return response;
        }

        public async Task<Response> GetAllEmployeesAsync()
        {
            Response response = new Response();

            try
            {
                var employees = await _db.Employees.ToListAsync();
                response.IsSuccess = false;
                response.Message = "Сотрудники не найдены.";

                if (employees.Count != 0)
                {
                    response.IsSuccess = true;
                    response.Result = employees;
                    response.Message = "Сотрудники успешно получены.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors.Add(ex.Message);
            }

            return response;
        }

        public async Task<Response> GetEmployeeByIdAsync(int id)
        {
            Response response = new Response();
            var employee = await _db.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id);

            response.IsSuccess = false;
            response.Message = "Сотрудник не найден.";
            if (employee != null)
            {
                response.IsSuccess = true;
                response.Result = employee;
                response.Message = "Сотрудник найден.";

            }

            return response;
        }

        public async Task<Response> UpdateEmployeeAsync(int id, EmployeeDto employeeDto)
        {
            Response response = new Response();
            var employee = await _db.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id);

            response.IsSuccess = false;
            response.Message = "Сотрудник не найден.";

            if (employee != null)
            {
                if (!string.IsNullOrEmpty(employeeDto.FullName))
                    employee.FullName = employeeDto.FullName;

                if (employeeDto.CompanyId != 0)
                    employee.StockId = employeeDto.CompanyId;

                if (!string.IsNullOrEmpty(employeeDto.JobTitle))
                    employee.JobTitle = employeeDto.JobTitle;

                if (!string.IsNullOrEmpty(employeeDto.Photo))
                    employee.Photo = employeeDto.Photo;

                if (!string.IsNullOrEmpty(employeeDto.Email))
                    employee.Email = employeeDto.Email;

                if (!string.IsNullOrEmpty(employeeDto.Phone))
                    employee.Phone = employeeDto.Phone;


                await _db.SaveChangesAsync();

                response.IsSuccess = true;
                response.Result = employee;
                response.Message = "Данные сотрудника обновлены.";
            }

            return response;
        }
    }
}
