using Microsoft.AspNetCore.Mvc;
using StockService.Models;
using StockService.Models.dto;
using StockService.Repository.UpdRep;

namespace StockService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdController : ControllerBase
    {
        private readonly IUpdService _updService;
        private Response _response;

        public UpdController(IUpdService updService)
        {
            _updService = updService;
            this._response = new Response();
        }

        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateUpd([FromForm] UpdDto updDto)
        {
            try
            {
                _response = await _updService.CreateUpdAsync(updDto);
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

        [HttpDelete("delById")]
        public async Task<IActionResult> DeleteUpd([FromQuery] int updId)
        {
            try
            {
                _response = await _updService.DeleteUpdAsync(updId);
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
        public async Task<IActionResult> GetAllUpds()
        {
            try
            {
                _response = await _updService.GetAllUpdsAsync();
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

        [HttpGet("getInRange")]
        public async Task<IActionResult> GetUpdsInRange([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, bool ascending = true)
        {
            try
            {
                var updDto = new UpdDto { StartDate = startDate, EndDate = endDate, Ascending = ascending };

                _response = await _updService.GetUpdsInRangeAsync(updDto);
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

        [HttpGet("getByProviderId")]
        public async Task<IActionResult> GetUpdsByProviderId([FromQuery] int providerId)
        {
            try
            {
                _response = await _updService.GetUpdsByProviderIdAsync(providerId);
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
        public async Task<IActionResult> GetUpdByIdAsync([FromQuery] int updId)
        {
            try
            {
                _response = await _updService.GetUpdByIdAsync(updId);
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

        [HttpPost("getUpdsFiltered")]
        public async Task<IActionResult> GetUpdsFiltered(UpdDto updDto)
        {
            try
            {
                _response = await _updService.GetUpdsFilteredAsync(updDto);
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
