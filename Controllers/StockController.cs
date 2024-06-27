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

        [HttpPost]
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

        [HttpDelete]
        public async Task<IActionResult> DeleteStockAsync(int id)
        {
            try
            {
                _response = await _stockService.DeleteStockAsync(id);
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


        [HttpGet("{id}")]
        public async Task<IActionResult> GetStockByIdAsync(int id)
        {
            try
            {
                _response = await _stockService.GetStockByIdAsync(id);

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
        public async Task<IActionResult> UpdateStockAsync(int id, StockDto stockDto)
        {
            try
            {
                var response = await _stockService.UpdateStockAsync(id, stockDto);
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
