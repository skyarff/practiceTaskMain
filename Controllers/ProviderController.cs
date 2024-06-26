using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockService.Models;
using StockService.Models.dto;
using StockService.Repository.EmployeeRep;
using StockService.Repository.ProviderRep;

namespace StockService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderController : ControllerBase
    {
        private readonly IProviderService _providerService;
        private Response _response;

        public ProviderController(IProviderService providerService)
        {
            _providerService = providerService;
            this._response = new Response();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(ProviderDto providerDto)
        {
            try
            {
                _response = await _providerService.CreateProviderAsync(providerDto);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmployeeAsync(int id)
        {
            try
            {
                _response = await _providerService.DeleteProviderAsync(id);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                _response = await _providerService.GetAllProvidersAsync();

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


        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            try
            {
                _response = await _providerService.GetProviderByIdAsync(id);

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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployeeAsync(int id, ProviderDto providerDto)
        {
            try
            {
                var response = await _providerService.UpdateProviderAsync(id, providerDto);
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
