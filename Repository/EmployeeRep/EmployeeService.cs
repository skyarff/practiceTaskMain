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
        public async Task<Response> ChangeEmployeePassword(int employeeId, string? password)
        {
            var employee = await _db.Employees.FindAsync(employeeId);

            _response.IsSuccess = false;
            _response.Message = "Не удалось установить новый пароль.";

            if (employee != null)
            {
                employee.Password = password;

                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Result = employee;

                _response.Message = "Пароль установлен в null.";

                if (password != null)
                {
                    _response.Message = "Пароль успешно изменен.";
                }
                
            }

            return _response;
        }

        public async Task<Response> CreateEmployeeAsync(EmployeeDto employeeDto)
        {
            var employeeIsExists = await _db.Employees.AnyAsync(e => e.Login == employeeDto.Login);

            _response.IsSuccess = false;
            _response.Message = "Пользователь уже существует.";
            if (!employeeIsExists)
            {
                var employee = _mapper.Map<EmployeeDto, Employee>(employeeDto);

                if (employeeDto.Image != null && employeeDto.Image.Length > 0)
                {
                    var fileName = Path.GetFileName(employeeDto.Image.FileName);
                    var filePath = Path.Combine("\\Images\\EmployeeImages", fileName);
                    

                    using (var stream = new FileStream(Path.Combine("wwwroot\\Images\\EmployeeImages", fileName), FileMode.Create))
                    {
                        await employeeDto.Image.CopyToAsync(stream);
                    }

                    employee.ImagePath = filePath;
                }

                _db.Employees.Add(employee);
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Result = employee;
                _response.Message = "Пользователь добавлен.";
            }

            return _response;
        }

        public async Task<Response> DeleteEmployeeAsync(int employeeId)
        {
            var employee = await _db.Employees.FindAsync(employeeId);

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

            var employees = await _db.Companies
                .Where(c => c.CompanyId == companyId)
                .SelectMany(c => c.Stocks.SelectMany(w => w.Employees))
                .ToListAsync();

            if (employees.Any())
            {
                _response.IsSuccess = true;
                _response.Result = employees;
                _response.Message = $"Сотрудники для компании с ID {companyId} успешно получены.";
            }

            return _response;
        }

        public async Task<Response> GetEmployeeByIdAsync(int employeeId)
        {
            var employee = await _db.Employees.FindAsync(employeeId);

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

        public async Task<Response> UpdateEmployeeAsync(int employeeId, EmployeeDto employeeDto)
        {
            var employee = await _db.Employees.FindAsync(employeeId);

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

                if (employeeDto.Image != null && employeeDto.Image.Length > 0)
                {

                    if (!string.IsNullOrEmpty(employee.ImagePath))
                    {
                        var oldImagePath = Path.Combine("wwwroot", employee.ImagePath.TrimStart('\\'));
                        if (File.Exists(oldImagePath))
                        {
                            File.Delete(oldImagePath);
                        }
                    }


                    var fileName = Path.GetFileName(employeeDto.Image.FileName);
                    var filePath = Path.Combine("\\Images\\EmployeeImages", fileName);


                    using (var stream = new FileStream(Path.Combine("wwwroot\\Images\\EmployeeImages", fileName), FileMode.Create))
                    {
                        await employeeDto.Image.CopyToAsync(stream);
                    }

                    employee.ImagePath = filePath;
                }

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
