using Microsoft.AspNetCore.Mvc;
using StockService.Models;
using StockService.Models.dto;
using StockService.Repository.BillRep;

namespace StockService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IBillService _billService;
        private Response _response;

        public BillController(IBillService billService)
        {
            _billService = billService;
            this._response = new Response();
        }

        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateStorageLocation([FromForm] BillDto billDto)
        {
            try
            {
                _response = await _billService.CreateBillAsync(billDto);
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
        public async Task<IActionResult> DeleteBill([FromQuery] int billId)
        {
            try
            {
                _response = await _billService.DeleteBillAsync(billId);
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
        public async Task<IActionResult> GetAllBills()
        {
            try
            {
                _response = await _billService.GetAllBillsAsync();
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
        public async Task<IActionResult> GetBillsInRange([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, bool ascending = true)
        {
            try
            {
                var billDto = new BillDto { StartDate = startDate, EndDate = endDate, Ascending = ascending };
                _response = await _billService.GetBillsInRangeAsync(billDto);
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
        public async Task<IActionResult> GetBillsByProviderId([FromQuery] int providerId)
        {
            try
            {
                _response = await _billService.GetBillsByProviderIdAsync(providerId);
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
        public async Task<IActionResult> GetBillByIdAsync([FromQuery] int billId)
        {
            try
            {
                _response = await _billService.GetBillByIdAsync(billId);
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
