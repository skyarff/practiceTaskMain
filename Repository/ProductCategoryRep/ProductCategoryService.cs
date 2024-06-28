using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockService.Models;
using StockService.Models.dto;

namespace StockService.Repository.ProductCategoryRep
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly StockContext _db;
        private readonly IMapper _mapper;
        private Response _response;

        public ProductCategoryService(IMapper mapper, StockContext db)
        {
            _db = db;
            _mapper = mapper;
            _response = new Response();
        }

        public async Task<Response> CreateProductCategoryAsync(ProductCategoryDto productCategoryDto)
        {
            var productCategoryIsExists = await _db.ProductCategories
                .AnyAsync(c => c.Name == productCategoryDto.Name && c.CompanyId == productCategoryDto.CompanyId);

            _response.IsSuccess = false;
            _response.Message = "Категория продуктов уже существует.";
            if (!productCategoryIsExists)
            {
                var productCategory = _mapper.Map<ProductCategoryDto, ProductCategory>(productCategoryDto);
                _db.ProductCategories.Add(productCategory);
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Result = productCategory;
                _response.Message = "Категория продуктов добавлена.";
            }
            return _response;
        }

        public async Task<Response> DeleteProductCategoryAsync(int productCategoryId)
        {
            var productCategory = await _db.ProductCategories.FindAsync(productCategoryId);

            _response.IsSuccess = false;
            _response.Message = "Категория продуктов не найдена.";

            if (productCategory != null)
            {
                _db.ProductCategories.Remove(productCategory);
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Message = "Категория продуктов удалена.";
            }

            return _response;
        }

        public async Task<Response> GetProductCategoriesByCompanyIdAsync(int companyId)
        {
            _response.IsSuccess = false;
            _response.Message = "Категории продуктов не найдены для указанной компании.";

            var stocks = await _db.ProductCategories
                    .Where(s => s.CompanyId == companyId)
                    .ToListAsync();

            if (stocks.Any())
            {
                _response.IsSuccess = true;
                _response.Result = stocks;
                _response.Message = $"Склады для компании с ID {companyId} успешно получены.";
            }

            return _response;
        }
    }
}
