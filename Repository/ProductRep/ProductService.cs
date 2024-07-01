using AppSettings;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockService.Models;
using StockService.Models.dto;
using System.Text.RegularExpressions;

namespace StockService.Repository.ProductRep
{
    public class ProductService : IProductService
    {
        private readonly StockContext _db;
        private readonly IMapper _mapper;
        private Response _response;
        private string _imagePath = PathSettings.ImagePaths["ProductImages"];
        public ProductService(IMapper mapper, StockContext db)
        {
            _db = db;
            _mapper = mapper;
            _response = new Response();
        }
        
        public async Task<Response> CreateProductAsync(ProductDto productDto)
        {
            var productIsExists = await _db.Products.AnyAsync(p => p.StorageLocationId == productDto.StorageLocationId);

            _response.IsSuccess = false;
            _response.Message = "Место хранения уже используется или данные конфликтуют.";
            if (!productIsExists)
            {
                var product = _mapper.Map<ProductDto, Product>(productDto);

                if (productDto.Image != null && productDto.Image.Length > 0)
                {
                    var fileName = Path.GetFileName(productDto.Image.FileName);
                    var filePath = $"{_imagePath}/{fileName}";


                    using (var stream = new FileStream("wwwroot/" + filePath, FileMode.Create))
                    {
                        await productDto.Image.CopyToAsync(stream);
                    }

                    product.ImagePath = filePath;
                }

                _db.Products.Add(product);
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Result = product;
                _response.Message = "Продукт добавлен.";
            }

            return _response;
        }

        public async Task<Response> DeleteProductAsync(int productId)
        {
            var product = await _db.Products.FindAsync(productId);

            _response.IsSuccess = false;
            _response.Message = "Продукт не найден.";

            if (product != null)
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Message = "Продукт успешно удален.";
            }

            return _response;
        }

        public async Task<Response> GetAllProductsAsync()
        {
            var products = await _db.Products.ToListAsync();

            _response.IsSuccess = false;
            _response.Message = "Продукты не найдены.";

            if (products.Any())
            {
                _response.IsSuccess = true;
                _response.Result = products;
                _response.Message = "Продукты получены.";
            }

            return _response;
        }

        public async Task<Response> GetProductByIdAsync(int productId)
        {
            var product = await _db.Products.FindAsync(productId);

            _response.IsSuccess = false;
            _response.Message = "Продукт не найден.";
            if (product != null)
            {
                _response.IsSuccess = true;
                _response.Result = product;
                _response.Message = "Продукт найден.";

            }

            return _response;
        }

        public async Task<Response> UpdateProductAsync(ProductDto productDto)
        {
            var product = await _db.Products.FindAsync(productDto.ProductId);

            _response.IsSuccess = false;
            _response.Message = "Продукт не найден.";

            if (product != null)
            {
                if (productDto.ProductCategoryId != null)
                {
                    var pc = await _db.ProductCategories.FindAsync(productDto.ProductCategoryId);

                    if (pc != null) product.ProductCategoryId = productDto.ProductCategoryId;
                }
                  
                if (productDto.Price != null) 
                    product.Price = (decimal)productDto.Price;

                if (!string.IsNullOrEmpty(productDto.Name))
                    product.Name = productDto.Name;

                if (!string.IsNullOrEmpty(productDto.Manufacturer))
                    product.Manufacturer = productDto.Manufacturer;

                if (!string.IsNullOrEmpty(productDto.ProductionArticle))
                    product.ProductionArticle = productDto.ProductionArticle;

                if (!string.IsNullOrEmpty(productDto.InnerArticle))
                    product.InnerArticle = productDto.InnerArticle;

                if (!string.IsNullOrEmpty(productDto.FactoryNumber))
                    product.FactoryNumber = productDto.FactoryNumber;

                if (productDto.BillId != null)
                    product.BillId = productDto.BillId;

                if (productDto.UpdId != null)
                    product.UpdId = productDto.UpdId;

                if (productDto.Image != null && productDto.Image.Length > 0)
                {

                    if (!string.IsNullOrEmpty(product.ImagePath))
                    {
                        var oldImagePath = Path.Combine("wwwroot", product.ImagePath.TrimStart('\\'));
                        if (File.Exists(oldImagePath))
                        {
                            File.Delete(oldImagePath);
                        }
                    }

                    var fileName = Path.GetFileName(productDto.Image.FileName);
                    var filePath = $"{_imagePath}/{fileName}";


                    using (var stream = new FileStream("wwwroot/" + filePath, FileMode.Create))
                    {
                        await productDto.Image.CopyToAsync(stream);
                    }

                    product.ImagePath = filePath;
                }


                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Result = product;
                _response.Message = "Данные продукта обновлены.";
            }

            return _response;
        }

        public async Task<Response> GetProductsFilteredAsync(ProductDto productDto)
        {
            _response.IsSuccess = false;
            _response.Message = "Продукты не найдены по указанным критериям.";

            var query = _db.Products.AsQueryable();


            if (productDto.LowerPriceLimit != null)
                query = query.Where(p => p.Price >= productDto.LowerPriceLimit);
            if (productDto.UpperPriceLimit != null)
                query = query.Where(p => p.Price <= productDto.UpperPriceLimit);

            if (!string.IsNullOrEmpty(productDto.Name))
                query = query.Where(p => Regex.IsMatch(p.Name, Regex.Escape(productDto.Name), RegexOptions.IgnoreCase));
            if (!string.IsNullOrEmpty(productDto.Manufacturer))
                query = query.Where(p => Regex.IsMatch(p.Manufacturer, Regex.Escape(productDto.Manufacturer), RegexOptions.IgnoreCase));
            if (!string.IsNullOrEmpty(productDto.ProductionArticle))
                query = query.Where(p => Regex.IsMatch(p.ProductionArticle, Regex.Escape(productDto.ProductionArticle), RegexOptions.IgnoreCase));
            if (!string.IsNullOrEmpty(productDto.InnerArticle))
                query = query.Where(p => Regex.IsMatch(p.InnerArticle, Regex.Escape(productDto.InnerArticle), RegexOptions.IgnoreCase));
            if (!string.IsNullOrEmpty(productDto.FactoryNumber))
                query = query.Where(p => Regex.IsMatch(p.FactoryNumber, Regex.Escape(productDto.FactoryNumber), RegexOptions.IgnoreCase));


            if (productDto.StartDate != null)
                query = query.Where(p => p.CreateDate >= productDto.StartDate.Value);
            if (productDto.EndDate != null)
                query = query.Where(p => p.CreateDate <= productDto.EndDate.Value);


            if (productDto.EmployeeId != null)
                query = query.Where(p => p.EmployeeId == productDto.EmployeeId);
            if (productDto.StorageLocationId != null)
                query = query.Where(p => p.StorageLocationId == productDto.StorageLocationId);
            if (productDto.ProductCategoryId != null)
                query = query.Where(p => p.ProductCategoryId == productDto.ProductCategoryId);


            if (productDto.StockId != null)
                query = query.Where(p => p.StorageLocation.StockId == productDto.StockId);
            else if(productDto.CompanyId != null)
                query = query.Where(p => p.StorageLocation.Stock.CompanyId == productDto.CompanyId);


            if (productDto.BillId != null)
                query = query.Where(p => p.BillId == productDto.BillId);
            else if (productDto.UpdId != null)
                query = query.Where(p => p.UpdId == productDto.UpdId);
            else if (productDto.ProviderId != null)
                query = query.Where(p => p.Bill.ProviderId == productDto.ProviderId || p.Upd.ProviderId == productDto.ProviderId);


            var products = await query.ToListAsync();
            if (products.Any())
            {
                _response.IsSuccess = true;
                _response.Result = products;
                _response.Message = "Продукты успешно найдены по указанным критериям.";
            }

            return _response;
        }

    }
}
