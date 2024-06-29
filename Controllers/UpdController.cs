using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockService.Models;
using StockService.Models.dto;
using StockService.Repository.BillRep;
using StockService.Repository.EmployeeRep;
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
        public async Task<IActionResult> CreateStorageLocation([FromForm] UpdDto updDto)
        {
            try
            {
                _response = await _updService.CreateUpdAsync(updDto);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
        }

        [HttpDelete("delById/{updId}")]
        public async Task<IActionResult> DeleteBill(int updId)
        {
            try
            {
                _response = await _updService.DeleteUpdAsync(updId);
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
        public async Task<IActionResult> GetAllUpds()
        {
            try
            {
                _response = await _updService.GetAllUpdsAsync();

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

        [HttpPost("getInRange")]
        public async Task<IActionResult> GetUpdsInRange(UpdDto updDto)
        {
            try
            {
                _response = await _updService.GetUpdsInRangeAsync(updDto);

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

        [HttpGet("getByProviderId/{providerId}")]
        public async Task<IActionResult> GetUpdsByProviderId(int providerId)
        {
            try
            {
                _response = await _updService.GetUpdsByProviderIdAsync(providerId);

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

        [HttpGet("getById/{updId}")]
        public async Task<IActionResult> GetUpdByIdAsync(int updId)
        {
            try
            {
                _response = await _updService.GetUpdByIdAsync(updId);

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
    }
}
