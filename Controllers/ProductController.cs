using Microsoft.AspNetCore.Mvc;
using StockService.Models;
using StockService.Models.dto;
using StockService.Repository.ProductRep;

namespace StockService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private Response _response;

        public ProductController(IProductService productService)
        {
            _productService = productService;
            this._response = new Response();
        }

        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductDto productDto)
        {
            try
            {
                _response = await _productService.CreateProductAsync(productDto);
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

        [HttpDelete("dellById/")]
        public async Task<IActionResult> DeleteProduct([FromQuery] int productId)
        {
            try
            {
                _response = await _productService.DeleteProductAsync(productId);
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
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                _response = await _productService.GetAllProductsAsync();
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
        public async Task<IActionResult> GetProductById([FromQuery] int productId)
        {
            try
            {
                _response = await _productService.GetProductByIdAsync(productId);
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

        [HttpGet("getProductsFiltered")]
        public async Task<IActionResult> GetProductsFiltered(ProductDto productDto)
        {
            try
            {
                _response = await _productService.GetProductsFilteredAsync(productDto);
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
        public async Task<IActionResult> UpdateProductAsync([FromForm] ProductDto productDto)
        {
            try
            {
                _response = await _productService.UpdateProductAsync(productDto);
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
    }
}
