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

        [Authorize(Roles = "CompanyLevelWorker,Admin")]
        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateEmployee([FromForm] EmployeeDto employeeDto)
        {
            try
            {
                _response = await _employeeService.CreateEmployeeAsync(employeeDto, User);
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

        [Authorize(Roles = "CompanyLevelWorker,Admin")]
        [HttpDelete("dellById")]
        public async Task<IActionResult> DeleteEmployee([FromQuery] int employeeId)
        {
            try
            {
                _response = await _employeeService.DeleteEmployeeAsync(employeeId, User);
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "CompanyLevelWorker,Admin")]
        [HttpGet("getById")]
        public async Task<IActionResult> GetEmployeeById([FromQuery] int employeeId)
        {
            try
            {
                _response = await _employeeService.GetEmployeeByIdAsync(employeeId, User);
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

        [Authorize(Roles = "StockLevelWorker,CompanyLevelWorker,Admin")]
        [HttpGet("getByStockId")]
        public async Task<IActionResult> GetEmployeesByStockIdAsync([FromQuery] int? stockId)
        {
            try
            {
                _response = await _employeeService.GetEmployeesByStockIdAsync(stockId, User);
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

        [Authorize(Roles = "CompanyLevelWorker,Admin")]
        [HttpGet("getByCompanyId")]
        public async Task<IActionResult> GetEmployeesByCompanyIdAsync([FromQuery] int? companyId)
        {
            try
            {
                _response = await _employeeService.GetEmployeesByCompanyIdAsync(companyId, User);
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

        [Authorize(Roles = "StockLevelWorker,CompanyLevelWorker,Admin")]
        [HttpPut("update")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateEmployeeAsync([FromForm] EmployeeDto employeeDto)
        {
            try
            {
                _response = await _employeeService.UpdateEmployeeAsync(employeeDto, User);
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

        [Authorize(Roles = "CompanyLevelWorker,Admin")]
        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangeEmployeePassword(EmployeeDto employeeDto)
        {
            try
            {
                _response = await _employeeService.ChangeEmployeePassword(employeeDto, User);
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
        public async Task<IActionResult> GetEmployeesFiltered(EmployeeDto employeeDto)
        {
            try
            {
                _response = await _employeeService.GetEmployeesFilteredAsync(employeeDto, User);
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

        [Authorize(Roles = "StockLevelWorker,CompanyLevelWorker,Admin")]
        [HttpGet("getEmployeeInfo")]
        public async Task<IActionResult> GetEmployeeInfo()
        {
            try
            {
                _response = await _employeeService.GetEmployeeInfo(User);
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

        [AllowAnonymous]
        [HttpPost("signIn")]
        public async Task<IActionResult> SignIn([FromBody] EmployeeDto employee)
        {
            try
            {
                _response = await _employeeService.SignInAsync(employee);
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

        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<IActionResult> logout()
        {
            try
            {
                _response = await _employeeService.Logout();
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

        [AllowAnonymous]
        [HttpPost("refreshTokens")]
        public async Task<IActionResult> RefreshTokens()
        {
            try
            {
                TokenPair tokenPair = new TokenPair()
                {
                    AccessToken = Request.Cookies["accessToken"],
                    RefreshToken = Request.Cookies["refreshToken"],
                };

                _response = await _employeeService.GetNewTokenPairAsync(tokenPair);
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
