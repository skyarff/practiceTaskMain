using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockService.Models;
using StockService.Models.dto;
using StockService.Repository.ProductCategoryRep;
using System.Data;

namespace StockService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategory : ControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;
        private Response _response;


        public ProductCategory(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
            this._response = new Response();
        }

        [Authorize(Roles = "CompanyLevelWorker,Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateProductCategory(ProductCategoryDto productCategoryDto)
        {
            try
            {
                _response = await _productCategoryService.CreateProductCategoryAsync(productCategoryDto, User);
                if (_response.IsSuccess) return Ok(_response);
                return NotFound(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add($"An error occurred while saving the entity changes: {ex.Message}");

                if (ex.InnerException != null)
                {
                    _response.Errors.Add($"Inner exception: {ex.InnerException.Message}");
                }

                return BadRequest(_response);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllProductCategories()
        {
            try
            {
                _response = await _productCategoryService.GetAllProductCategoriesAsync();
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
        public async Task<IActionResult> DeleteProductCategory([FromQuery] int productCategoryId)
        {
            try
            {
                _response = await _productCategoryService.DeleteProductCategoryAsync(productCategoryId, User);
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
        [HttpGet("getByCompany")]
        public async Task<IActionResult> GetProductCategoriesByCompanyId([FromQuery] int companyId)
        {
            try
            {
                _response = await _productCategoryService.GetProductCategoriesByCompanyIdAsync(companyId, User);
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
        [HttpPost("getProductCategoriesFiltered")]
        public async Task<IActionResult> GetProductCategoriesFiltered(ProductCategoryDto productCategoryDto)
        {
            try
            {
                _response = await _productCategoryService.GetCategoriesFilteredAsync(productCategoryDto, User);
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
