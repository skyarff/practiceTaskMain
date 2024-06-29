using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockService.Models;
using StockService.Models.dto;
using StockService.Repository.ProductCategoryRep;

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


        [HttpPost("create")]
        public async Task<IActionResult> CreateProductCategory(ProductCategoryDto productCategoryDto)
        {
            try
            {
                _response = await _productCategoryService.CreateProductCategoryAsync(productCategoryDto);
                return Ok(_response);
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

        [HttpDelete("dellById/{productCategoryId}")]
        public async Task<IActionResult> DeleteProductCategory(int productCategoryId)
        {
            try
            {
                _response = await _productCategoryService.DeleteProductCategoryAsync(productCategoryId);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
        }

        [HttpGet("getByCompany/{companyId}")]
        public async Task<IActionResult> GetStocksByCompanyId(int companyId)
        {
            try
            {
                _response = await _productCategoryService.GetProductCategoriesByCompanyIdAsync(companyId);

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
