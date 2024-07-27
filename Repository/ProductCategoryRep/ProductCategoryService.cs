using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockService.Models;
using StockService.Models.dto;
using System.ComponentModel.Design;
using System.Security.Claims;

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

        public async Task<Response> CreateProductCategoryAsync(ProductCategoryDto productCategoryDto, ClaimsPrincipal User)
        {
            if (User.IsInRole("CompanyLevelWorker")
                && (productCategoryDto.CompanyId = Convert.ToInt32(User.FindFirstValue("CompanyId"))) == -1
                )
                throw new Exception("Некорректные данные запроса");


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
        public async Task<Response> DeleteProductCategoryAsync(int productCategoryId, ClaimsPrincipal User)
        {
            var productCategory = await _db.ProductCategories.FindAsync(productCategoryId);

            if (User.IsInRole("CompanyLevelWorker")
                && productCategory?.CompanyId != Convert.ToInt32(User.FindFirstValue("CompanyId")))
                throw new Exception("Некорректные данные запроса");


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
        public async Task<Response> GetAllProductCategoriesAsync()
        {
            _response.IsSuccess = false;
            _response.Message = "Категории продуктов не найдены.";

            var stocks = await _db.ProductCategories.ToListAsync();

            if (stocks.Any())
            {
                _response.IsSuccess = true;
                _response.Result = stocks;
                _response.Message = $"Категории успешно получены.";
            }

            return _response;
        }
        public async Task<Response> GetProductCategoriesByCompanyIdAsync(int companyId, ClaimsPrincipal User)
        {
            if ((User.IsInRole("CompanyLevelWorker") || User.IsInRole("StockLevelWorker"))
                && companyId != Convert.ToInt32(User.FindFirstValue("CompanyId")))
                throw new Exception("Некорректные данные запроса");

            _response.IsSuccess = false;
            _response.Message = "Категории продуктов не найдены для указанной компании.";

            var stocks = await _db.ProductCategories
                    .Where(s => s.CompanyId == companyId)
                    .ToListAsync();

            if (stocks.Any())
            {
                _response.IsSuccess = true;
                _response.Result = stocks;
                _response.Message = $"Категории для компании с ID {companyId} успешно получены.";
            }

            return _response;
        }
        public async Task<Response> GetCategoriesFilteredAsync(ProductCategoryDto productCategoryDto, ClaimsPrincipal User)
        {
            if ((User.IsInRole("CompanyLevelWorker") || User.IsInRole("StockLevelWorker"))
                && (productCategoryDto.CompanyId = Convert.ToInt32(User.FindFirstValue("CompanyId"))) == -1)
                throw new Exception("Некорректные данные запроса");

            _response.IsSuccess = false;
            _response.Message = "Категории продуктов не найдены по указанным критериям.";

            var query = _db.ProductCategories.AsQueryable();

            if (productCategoryDto.ProductCategoryId != null)
                query = query.Where(pc => pc.ProductCategoryId == productCategoryDto.ProductCategoryId);

            if (!string.IsNullOrEmpty(productCategoryDto.Name))
                query = query.Where(pc => EF.Functions.ILike(pc.Name, $"%{productCategoryDto.Name}%"));
            if (productCategoryDto.CompanyId != null)
                query = query.Where(pc => pc.CompanyId == productCategoryDto.CompanyId);


            //var productCategories = await query.ToListAsync();

            #region
            var productCategories = await query
                .Select(pc => new
                {
                    ProductCategoryId = pc.ProductCategoryId,
                    Name = pc.Name,
                    CompanyId = pc.CompanyId,
                    CompanyName = _db.Companies
                        .Where(c => c.CompanyId == pc.CompanyId)
                        .Select(c => c.Name)
                        .FirstOrDefault(),
                })
                .ToListAsync();
            #endregion

            if (productCategories.Any())
            {
                _response.IsSuccess = true;
                _response.Result = productCategories;
                _response.Message = "Категории продуктов успешно найдены по указанным критериям.";
            }

            return _response;
        }
    }
}
