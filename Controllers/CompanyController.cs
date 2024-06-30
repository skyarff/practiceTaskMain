using Microsoft.AspNetCore.Mvc;
using StockService.Models;
using StockService.Models.dto;
using StockService.Repository.CompanyRep;

namespace StockService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private Response _response;


        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
            this._response = new Response();
        }

        
        [HttpPost("create")]
        public async Task<IActionResult> CreateCompany(CompanyDto companyDto)
        {
            try
            {
                _response = await _companyService.CreateCompanyAsync(companyDto);
                if (_response.IsSuccess) return Ok(_response);
                return NotFound(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add($"An error occurred while saving the entity changes: {ex.Message}");

                if (ex.InnerException != null)
                {
                    _response.Errors.Add($"Inner exception: {ex.InnerException.Message}");
                }

                return BadRequest(_response);
            }
        }

        [HttpDelete("dellById")]
        public async Task<IActionResult> DeleteCompany([FromQuery] int companyId)
        {
            try
            {
                _response = await _companyService.DeleteCompanyAsync(companyId);
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
                _response = await _companyService.GetAllCompaniesAsync();
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
        public async Task<IActionResult> GetCompanyById([FromQuery] int companyId)
        {
            try
            {
                _response = await _companyService.GetCompanyByIdAsync(companyId);
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
        public async Task<IActionResult> UpdateEmployeeAsync(CompanyDto companyDto)
        {
            try
            {
                _response = await _companyService.UpdateCompanyAsync(companyDto);
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
    }
}
