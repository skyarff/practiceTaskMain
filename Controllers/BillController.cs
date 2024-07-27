using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "CompanyLevelWorker,Admin")]
        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateBill([FromForm] BillDto billDto)
        {
            try
            {
                _response = await _billService.CreateBillAsync(billDto, User);
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
        public async Task<IActionResult> DeleteBill([FromQuery] int billId)
        {
            try
            {
                _response = await _billService.DeleteBillAsync(billId, User);
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

        [Authorize(Roles = "CompanyLevelWorker,Admin")]
        [HttpGet("getByProviderAndCompanyId")]
        public async Task<IActionResult> GetBillsByProviderAndCompanyIdAsync([FromQuery] int? providerId, int? companyId)
        {
            try
            {
                _response = await _billService.GetBillsByProviderAndCompanyIdAsync(providerId, companyId, User);
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
        public async Task<IActionResult> GetBillByIdAsync([FromQuery] int billId)
        {
            try
            {
                _response = await _billService.GetBillByIdAsync(billId, User);
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
        [HttpPost("getBillsFiltered")]
        public async Task<IActionResult> GetBillsFiltered(BillDto billDto)
        {
            try
            {
                _response = await _billService.GetBillsFilteredAsync(billDto, User);
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
