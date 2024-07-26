using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockService.Models;
using StockService.Models.dto;
using StockService.Repository.StockRep;
using System.Data;

namespace StockService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;
        private Response _response;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
            this._response = new Response();
        }

        [Authorize(Roles = "CompanyLevelWorker,Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateStock(StockDto stockDto)
        {
            try
            {
                _response = await _stockService.CreateStockAsync(stockDto, User);
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
        public async Task<IActionResult> DeleteStock([FromQuery] int stockId)
        {
            try
            {
                _response = await _stockService.DeleteStockAsync(stockId, User);
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
        public async Task<IActionResult> GetAllStocks()
        {
            try
            {
                _response = await _stockService.GetAllStocksAsync();
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
        public async Task<IActionResult> GetStocksByCompanyId([FromQuery] int? companyId)
        {
            try
            {
                _response = await _stockService.GetStocksByCompanyIdAsync(companyId, User);
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
        public async Task<IActionResult> GetStockByIdAsync([FromQuery] int stockId)
        {
            try
            {
                _response = await _stockService.GetStockByIdAsync(stockId, User);
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
        [HttpPut("update")]
        public async Task<IActionResult> UpdateStockAsync(StockDto stockDto)
        {
            try
            {
                _response = await _stockService.UpdateEmployeeAsync(stockDto, User);
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
        [HttpPost("getStocksFiltered")]
        public async Task<IActionResult> GetStocksFiltered(StockDto stockDto)
        {
            try
            {
                _response = await _stockService.GetStocksFilteredAsync(stockDto, User);
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
