using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockService.Models;
using StockService.Models.dto;
using StockService.Repository.UpdRep;
using System.Data;

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

        [Authorize(Roles = "StockLevelWorker,CompanyLevelWorker,Admin")]
        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateUpd([FromForm] UpdDto updDto)
        {
            try
            {
                _response = await _updService.CreateUpdAsync(updDto, User);
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
        [HttpDelete("delById")]
        public async Task<IActionResult> DeleteUpd([FromQuery] int updId)
        {
            try
            {
                _response = await _updService.DeleteUpdAsync(updId, User);
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

        [Authorize(Roles = "StockLevelWorker,CompanyLevelWorker,Admin")]
        [HttpGet("getByBillId")]
        public async Task<IActionResult> GetUpdsByBillId([FromQuery] int billId)
        {
            try
            {
                _response = await _updService.GetUpdsByBillIdAsync(billId, User);
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
        [HttpGet("getById")]
        public async Task<IActionResult> GetUpdByIdAsync([FromQuery] int updId)
        {
            try
            {
                _response = await _updService.GetUpdByIdAsync(updId, User);
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
        [HttpPost("getUpdsFiltered")]
        public async Task<IActionResult> GetUpdsFiltered(UpdDto updDto)
        {
            try
            {
                _response = await _updService.GetUpdsFilteredAsync(updDto, User);
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
