using AppSettings;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StockService.Models;
using StockService.Models.dto;
using StockService.Repository.CookieRep;
using StockService.Repository.JwtRep;
using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StockService.Repository.EmployeeRep
{
    public class EmployeeService : IEmployeeService
    {
        private readonly StockContext _db;
        private readonly IMapper _mapper;
        private Response _response;
        private string _imagePath = PathSettings.ImagePaths["EmployeeImages"];

        private readonly ITokenService _tokenService;
        private readonly ICookieService _cookieService;

        public EmployeeService(IMapper mapper, 
            ITokenService tokenService, 
            ICookieService cookieService, 
            StockContext db)
        {
            _db = db;
            _mapper = mapper;
            _tokenService = tokenService;
            _cookieService = cookieService;
            _response = new Response();
        }
        public async Task<Response> ChangeEmployeePassword(EmployeeDto employeeDto, ClaimsPrincipal User)
        {
            var employee = await _db.Employees.FindAsync(employeeDto.EmployeeId);

            if (User.IsInRole("CompanyLevelWorker")
                    && ((employee.CompanyId != Convert.ToInt32(User.FindFirstValue("CompanyId"))
                    || employee.Role == "CompanyLevelWorker")
                    || employee.Role == "Admin"))
                throw new Exception("Некорректные данные запроса");


            _response.IsSuccess = false;
            _response.Message = "Не удалось установить новый пароль.";

            if (employee != null)
            {
                if (string.IsNullOrEmpty(employeeDto.Password))
                {
                    employee.PasswordHash = null;
                    _response.Message = "Пароль установлен в null.";
                }
                else
                {
                    employee.PasswordHash = Sha256.ComputeSha256Hash(employeeDto.Password);
                    _response.Message = "Пароль успешно изменен.";
                }

                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Result = employee;
            }

            return _response;
        }

        public async Task<Response> CreateEmployeeAsync(EmployeeDto employeeDto, ClaimsPrincipal User)
        {
            var stock = await _db.Stocks.FindAsync(employeeDto.StockId);

            if (User.IsInRole("CompanyLevelWorker") 
                && ((employeeDto.Role = "StockLevelWorker") == ""
                || stock?.CompanyId != Convert.ToInt32(User.FindFirstValue("CompanyId")))
                )
                throw new Exception("Некорректные данные запроса");


            var employeeIsExists = await _db.Employees.AnyAsync(e => e.Login == employeeDto.Login);

            _response.IsSuccess = false;
            _response.Message = "Пользователь уже существует.";
            if (!employeeIsExists)
            {
                var employee = _mapper.Map<EmployeeDto, Employee>(employeeDto);


                if (stock != null) employee.CompanyId = stock.CompanyId;

                if (!string.IsNullOrEmpty(employeeDto.Password))
                    employee.PasswordHash = Sha256.ComputeSha256Hash(employeeDto.Password);

                string filePath = "";
                if (employeeDto.Image != null && employeeDto.Image.Length > 0)
                {
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(employeeDto.Image.FileName)}";
                    filePath = Path.Combine(_imagePath, fileName);
                    employee.ImagePath = filePath;
                }

                _db.Employees.Add(employee);
                await _db.SaveChangesAsync();

                if (!string.IsNullOrEmpty(filePath))
                    using (var stream = new FileStream("wwwroot/" + filePath, FileMode.Create))
                        await employeeDto.Image.CopyToAsync(stream);

                _response.IsSuccess = true;
                _response.Result = employee;
                _response.Message = "Пользователь добавлен.";
            }

            return _response;
        }

        public async Task<Response> DeleteEmployeeAsync(int employeeId, ClaimsPrincipal User)
        {
            var employee = await _db.Employees.FindAsync(employeeId);

            if (User.IsInRole("CompanyLevelWorker")
                    && ((employee.CompanyId != Convert.ToInt32(User.FindFirstValue("CompanyId"))
                    || employee.Role == "CompanyLevelWorker")
                    || employee.Role == "Admin"))
                throw new Exception("Некорректные данные запроса");


            _response.IsSuccess = false;
            _response.Message = "Сотрудник не найден.";

            if (employee != null)
            {
                _db.Employees.Remove(employee);

                if (File.Exists("wwwroot//" + employee.ImagePath))
                    File.Delete("wwwroot//" + employee.ImagePath);

                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Message = "Сотрудник успешно удален.";
            }

            return _response;
        }

        public async Task<Response> GetAllEmployeesAsync()
        {
            var employees = await _db.Employees.ToListAsync();

            _response.IsSuccess = false;
            _response.Message = "Сотрудники не найдены.";

            if (employees.Any())
            {
                _response.IsSuccess = true;
                _response.Result = employees;
                _response.Message = "Сотрудники получены.";
            }

            return _response;
        }

        public async Task<Response> GetEmployeesByStockIdAsync(int? stockId, ClaimsPrincipal User)
        {
            var stock = await _db.Stocks.FindAsync(stockId);

            if (User.IsInRole("CompanyLevelWorker") 
                && stock?.CompanyId != Convert.ToInt32(User.FindFirstValue("CompanyId")))
                throw new Exception("Некорректные данные запроса");

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

        public async Task<Response> GetEmployeesByCompanyIdAsync(int? companyId, ClaimsPrincipal User)
        {
            if (User.IsInRole("CompanyLevelWorker")
                && companyId != Convert.ToInt32(User.FindFirstValue("CompanyId")))
                throw new Exception("Некорректные данные запроса");

            _response.IsSuccess = false;
            _response.Message = "Сотрудники не найдены для указанной компании.";

            var employees = await _db.Employees
                .Where(e => e.CompanyId == companyId)
                .ToListAsync();

            if (employees.Any())
            {
                _response.IsSuccess = true;
                _response.Result = employees;
                _response.Message = $"Сотрудники для компании с ID {companyId} успешно получены.";
            }

            return _response;
        }

        public async Task<Response> GetEmployeeByIdAsync(int employeeId, ClaimsPrincipal User)
        {
            var employee = await _db.Employees.FindAsync(employeeId);

            if (User.IsInRole("CompanyLevelWorker")
                   && (employee.CompanyId != Convert.ToInt32(User.FindFirstValue("CompanyId"))
                   || employee.Role == "Admin"))
                throw new Exception("Некорректные данные запроса");

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

        public async Task<Response> UpdateEmployeeAsync(EmployeeDto employeeDto, ClaimsPrincipal User)
        {
            var employee = await _db.Employees.FindAsync(employeeDto.EmployeeId);

            if (User.IsInRole("StockLevelWorker") && employee?.Role != "StockLevelWorker"
                || employee?.Role == User.FindFirstValue(ClaimTypes.Role)
                && (employee?.EmployeeId.ToString() != User.FindFirstValue(ClaimTypes.NameIdentifier))
                || User.IsInRole("CompanyLevelWorker")
                && (employee?.Role != "StockLevelWorker"
                || employee?.CompanyId != Convert.ToInt32(User.FindFirstValue("CompanyId")))
                )
                throw new Exception("Некорректные данные запроса");


            _response.IsSuccess = false;
            _response.Message = "Сотрудник не найден.";

            if (employee != null)
            {
                if (!string.IsNullOrEmpty(employeeDto.FullName))
                    employee.FullName = employeeDto.FullName;

                if (!string.IsNullOrEmpty(employee.Role)
                    && !User.IsInRole("StockLevelWorker")
                    && User.FindFirstValue(ClaimTypes.Role) != employee.Role
                    && employee.Role != "Admin"
                    )
                    employee.Role = employeeDto.Role;

                if (!string.IsNullOrEmpty(employeeDto.JobTitle))
                    employee.JobTitle = employeeDto.JobTitle;

                if (!string.IsNullOrEmpty(employeeDto.Password))
                    employee.PasswordHash = Sha256.ComputeSha256Hash(employeeDto.Password);

                string filePath = "";
                string? oldPath = employee.ImagePath;
                if (employeeDto.Image != null && employeeDto.Image.Length > 0)
                {
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(employeeDto.Image.FileName)}";
                    filePath = Path.Combine(_imagePath, fileName);
                    employee.ImagePath = filePath;
                }

                if (!string.IsNullOrEmpty(employeeDto.Email))
                    employee.Email = employeeDto.Email;

                if (!string.IsNullOrEmpty(employeeDto.Phone))
                    employee.Phone = employeeDto.Phone;


                await _db.SaveChangesAsync();

                if (!string.IsNullOrEmpty(filePath))
                {
                    using (var stream = new FileStream("wwwroot/" + filePath, FileMode.Create))
                        await employeeDto.Image.CopyToAsync(stream);

                    if (!string.IsNullOrEmpty(oldPath)
                        && File.Exists("wwwroot//" + oldPath))
                            File.Delete("wwwroot//" + oldPath);
                }

                _response.IsSuccess = true;
                _response.Result = employee;
                _response.Message = "Данные сотрудника обновлены.";
            }

            return _response;
        }

        public async Task<Response> GetEmployeesFilteredAsync(EmployeeDto employeeDto, ClaimsPrincipal User)
        {
            if (User.IsInRole("CompanyLevelWorker")
                    && (employeeDto.CompanyId = Convert.ToInt32(User.FindFirstValue("CompanyId"))) == -1)
                throw new Exception("Некорректные данные запроса");

            _response.IsSuccess = false;
            _response.Message = "Сотрудники не найдены по указанным критериям.";

            var query = _db.Employees.AsQueryable();

            if (employeeDto.EmployeeId != null)
                query = query.Where(e => e.EmployeeId == employeeDto.EmployeeId);

            if (!string.IsNullOrEmpty(employeeDto.Login))
                query = query.Where(e => EF.Functions.ILike(e.Login, $"%{employeeDto.Login}%"));
            if (!string.IsNullOrEmpty(employeeDto.FullName))
                query = query.Where(e => EF.Functions.ILike(e.FullName, $"%{employeeDto.FullName}%"));
            if (!string.IsNullOrEmpty(employeeDto.JobTitle))
                query = query.Where(e => EF.Functions.ILike(e.JobTitle, $"%{employeeDto.JobTitle}%"));

            if (!string.IsNullOrEmpty(employeeDto.Email))
                query = query.Where(e => EF.Functions.ILike(e.Email, $"%{employeeDto.Email}%"));
            if (!string.IsNullOrEmpty(employeeDto.Phone))
                query = query.Where(e => EF.Functions.ILike(e.Phone, $"%{employeeDto.Phone}%"));

            if (!string.IsNullOrEmpty(employeeDto.Role))
                query = query.Where(e => e.Role == employeeDto.Role);


            if (employeeDto.StockId != null)
                query = query.Where(e => e.StockId == employeeDto.StockId);
            else if (employeeDto.CompanyId != null)
                query = query.Where(e => e.CompanyId == employeeDto.CompanyId);

            //var employees = await query.ToListAsync();

            #region
            var employees = await query
                .Select(e => new
                {
                    EmployeeId = e.EmployeeId,
                    FullName = e.FullName,
                    JobTitle = e.JobTitle,
                    Login = e.Login,
                    Role = e.Role,
                    ImagePath = e.ImagePath,
                    Email = e.Email,
                    Phone = e.Phone,

                    StockId = e.StockId,
                    StockName = e.StockId != null ? _db.Stocks
                        .Where(s => s.StockId == e.StockId)
                        .Select(s => s.Name)
                        .FirstOrDefault() : null,

                    CompanyId = e.CompanyId,
                    CompanyName = _db.Companies
                        .Where(c => c.CompanyId == e.CompanyId)
                        .Select(c => c.Name)
                        .FirstOrDefault(),
                })
                .ToListAsync();
            #endregion

            if (employees.Any())
            {
                _response.IsSuccess = true;
                _response.Result = employees;
                _response.Message = "Сотрудники успешно найдены по указанным критериям.";
            }

            return _response;
        }

        public async Task<Response> SignInAsync(EmployeeDto employeeDto)
        {
            _response.IsSuccess = false;
            _response.Message = "Некорректные учетные данные.";

            //var employee = await _db.Employees.FindAsync(employeeDto.EmployeeId);
            var employee = await _db.Employees
                .FirstOrDefaultAsync(e => e.Login == employeeDto.Login);

            if (employee == null
                || employeeDto.Password == null
                || employee.PasswordHash != Sha256.ComputeSha256Hash(employeeDto.Password)
                )
                return _response;

            if (employee != null)
            {


                var employeeRes = _db.Employees
                .Where(e => e.EmployeeId == employee.EmployeeId)
                .Select(e => new
                {
                    EmployeeId = e.EmployeeId,
                    FullName = e.FullName,
                    JobTitle = e.JobTitle,
                    Login = e.Login,
                    Role = e.Role,

                    ImagePath = e.ImagePath,
                    Email = e.Email,
                    Phone = e.Phone,

                    StockId = e.StockId,
                    StockName = e.StockId != null ? _db.Stocks
                        .Where(s => s.StockId == e.StockId)
                        .Select(s => s.Name)
                        .FirstOrDefault() : null,
                    CompanyId = e.CompanyId,
                    CompanyName = _db.Companies
                        .Where(c => c.CompanyId == e.CompanyId)
                        .Select(c => c.Name)
                        .FirstOrDefault(),
                })
                .FirstOrDefault();



                _cookieService.SetCookie("token", 
                    _tokenService.GenerateToken(employee).ToString(), 30);
                _cookieService.SetCookie("employee", JsonConvert.SerializeObject(employeeRes), 30);


                _response.IsSuccess = true;
                _response.Message = "Авторизация успешно пройдена.";
            }

            return _response;
        }
    }
}
