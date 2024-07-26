using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockService.Models;
using StockService.Models.dto;
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

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateProvider(ProviderDto providerDto)
        {
            try
            {
                _response = await _providerService.CreateProviderAsync(providerDto);
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
        [HttpDelete("dellById")]
        public async Task<IActionResult> DeleteProvider([FromQuery] int providerId)
        {
            try
            {
                _response = await _providerService.DeleteProviderAsync(providerId);
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
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllProviders()
        {
            try
            {
                _response = await _providerService.GetAllProvidersAsync();
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
        public async Task<IActionResult> GetProviderById([FromQuery] int id)
        {
            try
            {
                _response = await _providerService.GetProviderByIdAsync(id);
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
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProviderAsync(ProviderDto providerDto)
        {
            try
            {
                _response = await _providerService.UpdateProviderAsync(providerDto);
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
        [HttpPost("getProvidersFiltered")]
        public async Task<IActionResult> GetProvidersFiltered(ProviderDto providerDto)
        {
            try
            {
                _response = await _providerService.GetProvidersFilteredAsync(providerDto);
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
