using Microsoft.AspNetCore.Mvc;
using StockService.Models;
using StockService.Models.dto;
using StockService.Repository.StorageLocationRep;

namespace StockService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageLocationController : ControllerBase
    {
        private readonly IStorageLocationService _storageLocationService;
        private Response _response;

        public StorageLocationController(IStorageLocationService storageLocationService)
        {
            _storageLocationService = storageLocationService;
            this._response = new Response();
        }

        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateStorageLocation([FromForm] StorageLocationDto storageLocationDto)
        {
            try
            {
                _response = await _storageLocationService.CreateStorageLocationAsync(storageLocationDto);
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
        public async Task<IActionResult> DeletesStorageLocation([FromQuery] int storageLocationId)
        {
            try
            {
                _response = await _storageLocationService.DeleteStorageLocationAsync(storageLocationId);
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
        public async Task<IActionResult> GetStorageLocationById([FromQuery] int storageLocationId)
        {
            try
            {
                _response = await _storageLocationService.GetStorageLocationByIdAsync(storageLocationId);
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
        public async Task<IActionResult> GetStorageLocationsByStockIdAsync([FromQuery] int stockId)
        {
            try
            {
                _response = await _storageLocationService.GetStorageLocationsByStockIdAsync(stockId);
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
        public async Task<IActionResult> UpdateStorageLocationAsync([FromForm] StorageLocationDto storageLocationDto)
        {
            try
            {
                _response = await _storageLocationService.UpdateStorageLocationAsync(storageLocationDto);
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

        [HttpPost("getCompaniesFiltered")]
        public async Task<IActionResult> GetCompaniesFiltered(StorageLocationDto storageLocationDto)
        {
            try
            {
                _response = await _storageLocationService.GetStorageLocationsFilteredAsync(storageLocationDto);
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
