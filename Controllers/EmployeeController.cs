using Microsoft.AspNetCore.Mvc;
using StockService.Models;
using StockService.Models.dto;
using StockService.Repository.CookieRep;
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
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
        }

        [HttpDelete("dellById/{employeeId}")]
        public async Task<IActionResult> DeleteEmployeeAsync(int employeeId)
        {
            try
            {
                _response = await _employeeService.DeleteEmployeeAsync(employeeId);
                return Ok(_response);
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
                _response = await _employeeService.GetAllEmployessAsync();

                if (_response.IsSuccess)
                {
                    return Ok(_response);
                }
                else
                {
                    return NotFound(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
        }

        [HttpGet("getById/{employeeId}")]
        public async Task<IActionResult> GetEmployeeById(int employeeId)
        {
            try
            {
                _response = await _employeeService.GetEmployeeByIdAsync(employeeId);

                if (_response.IsSuccess)
                {
                    return Ok(_response);
                }
                else
                {
                    return NotFound(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
        }

        [HttpGet("getByStockId/{stockId}")]
        public async Task<IActionResult> GetEmployeesByStockIdAsync(int stockId)
        {
            try
            {
                _response = await _employeeService.GetEmployeesByStockIdAsync(stockId);

                if (_response.IsSuccess)
                {
                    return Ok(_response);
                }
                else
                {
                    return NotFound(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
        }

        [HttpGet("getByCompanyId/{companyId}")]
        public async Task<IActionResult> GetEmployeesByCompanyIdAsync(int companyId)
        {
            try
            {
                _response = await _employeeService.GetEmployeesByCompanyIdAsync(companyId);

                if (_response.IsSuccess)
                {
                    return Ok(_response);
                }
                else
                {
                    return NotFound(_response);
                }
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
                var response = await _employeeService.UpdateEmployeeAsync(employeeDto);
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                else
                {
                    return NotFound(response);
                }
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
                var response = await _employeeService.ChangeEmployeePassword(employeeDto);
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                else
                {
                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }
    }
}
