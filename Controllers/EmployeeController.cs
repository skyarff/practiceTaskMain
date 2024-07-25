using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockService.Models;
using StockService.Models.dto;
using StockService.Repository.EmployeeRep;

namespace StockService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private Response _response;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
            this._response = new Response();
        }

        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateEmployee([FromForm] EmployeeDto employeeDto)
        {
            try
            {
                _response = await _employeeService.CreateEmployeeAsync(employeeDto);
                if (_response.IsSuccess) return Ok(_response);
                return NotFound(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
        }

        [HttpDelete("dellById")]
        public async Task<IActionResult> DeleteEmployee([FromQuery] int employeeId)
        {
            try
            {
                _response = await _employeeService.DeleteEmployeeAsync(employeeId);
                if (_response.IsSuccess) return Ok(_response);
                return NotFound(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                _response = await _employeeService.GetAllEmployeesAsync();
                if (_response.IsSuccess) return Ok(_response);
                return NotFound(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
        }

        [HttpGet("getById")]
        public async Task<IActionResult> GetEmployeeById([FromQuery] int employeeId)
        {
            try
            {
                _response = await _employeeService.GetEmployeeByIdAsync(employeeId);
                if (_response.IsSuccess) return Ok(_response);
                return NotFound(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
        }

        [HttpGet("getByStockId")]
        public async Task<IActionResult> GetEmployeesByStockIdAsync([FromQuery] int? stockId)
        {
            try
            {
                _response = await _employeeService.GetEmployeesByStockIdAsync(stockId);
                if (_response.IsSuccess) return Ok(_response);
                return NotFound(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
        }

        [HttpGet("getByCompanyId")]
        public async Task<IActionResult> GetEmployeesByCompanyIdAsync([FromQuery] int? companyId)
        {
            try
            {
                _response = await _employeeService.GetEmployeesByCompanyIdAsync(companyId);
                if (_response.IsSuccess) return Ok(_response);
                return NotFound(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
        }

        [HttpPut("update")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateEmployeeAsync([FromForm] EmployeeDto employeeDto)
        {
            try
            {
                _response = await _employeeService.UpdateEmployeeAsync(employeeDto);
                if (_response.IsSuccess) return Ok(_response);
                return NotFound(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangeEmployeePassword(EmployeeDto employeeDto)
        {
            try
            {
                _response = await _employeeService.ChangeEmployeePassword(employeeDto);
                if (_response.IsSuccess) return Ok(_response);
                return NotFound(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [Authorize(Roles = "StockLevelWorker,CompanyLevelWorker,Admin")]
        [HttpPost("getEmployeesFiltered")]
        public async Task<IActionResult> GetCompaniesFiltered(EmployeeDto employeeDto)
        {
            try
            {
                //if (User.IsInRole("StockLevelWorker"))
                //{
                //    int a = 5;
                //}
                //if (User.IsInRole("CompanyLevelWorker"))
                //{
                //    int a = 7;
                //}

                _response = await _employeeService.GetEmployeesFilteredAsync(employeeDto);
                if (_response.IsSuccess) return Ok(_response);
                return NotFound(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
        }


        
        [HttpGet("signIn")]
        public async Task<IActionResult> GetAllEmployees(string login, string password)
        {
            try
            {
                var loginData = new EmployeeDto() { Login = login, Password = password };
                _response = await _employeeService.SignInAsync(loginData);
                if (_response.IsSuccess) return Ok(_response);
                return NotFound(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
        }
    }
}
