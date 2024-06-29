using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockService.Models;
using StockService.Models.dto;
using StockService.Repository.EmployeeRep;
using StockService.Repository.StockRep;

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

        [HttpPost("create")]
        public async Task<IActionResult> CreateStock(StockDto stockDto)
        {
            try
            {
                _response = await _stockService.CreateStockAsync(stockDto);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
        }

        [HttpDelete("dellById/{stockId}")]
        public async Task<IActionResult> DeleteStockAsync(int stockId)
        {
            try
            {
                _response = await _stockService.DeleteStockAsync(stockId);
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
        public async Task<IActionResult> GetAllStocks()
        {
            try
            {
                _response = await _stockService.GetAllStocksAsync();

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
        public async Task<IActionResult> GetStocksByCompanyId(int companyId)
        {
            try
            {
                _response = await _stockService.GetStocksByCompanyIdAsync(companyId);

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

        [HttpGet("getById{stockId}")]
        public async Task<IActionResult> GetStockByIdAsync(int stockId)
        {
            try
            {
                _response = await _stockService.GetStockByIdAsync(stockId);

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
        public async Task<IActionResult> UpdateStockAsync(StockDto stockDto)
        {
            try
            {
                var response = await _stockService.ChangeStockCompanyAsync(stockDto);
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
