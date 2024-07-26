using AppSettings;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockService.Models;
using StockService.Models.dto;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

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
        
        public async Task<Response> CreateProductAsync(ProductDto productDto, ClaimsPrincipal User)
        {
            var storageLocation = await _db.StorageLocations.FindAsync(productDto.StorageLocationId);

            if (User.IsInRole("StockLevelWorker")
                && productDto.StockId != Convert.ToInt32(User.FindFirstValue("StockId"))
                || User.IsInRole("CompanyLevelWorker")
                && storageLocation?.CompanyId != Convert.ToInt32(User.FindFirstValue("CompanyId"))
                )
                throw new Exception("Некорректные данные запроса");

            var productIsExists = await _db.Products.AnyAsync(p => p.StorageLocationId == productDto.StorageLocationId
                && p.ShelfCode == productDto.ShelfCode);

            _response.IsSuccess = false;
            _response.Message = "Полка уже используется или данные конфликтуют.";
            if (!productIsExists)
            {
                var product = _mapper.Map<ProductDto, Product>(productDto);
                product.StockId = storageLocation.StockId;
                product.CompanyId = storageLocation.CompanyId;
                product.RackCode = storageLocation.RackCode;


                var upd = await _db.Upds.FindAsync(productDto.UpdId);
                if (upd != null && upd.CompanyId == product.CompanyId)
                {
                    product.BillId = upd.BillId;
                    product.ProviderId = upd.ProviderId;
                }
                else throw new Exception("УПД не обнаружен у данной компании.");

                


                string filePath = "";
                if (productDto.Image != null && productDto.Image.Length > 0)
                {
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(productDto.Image.FileName)}";
                    filePath = Path.Combine(_imagePath, fileName);
                    product.ImagePath = filePath;
                }


                product.CreateDate = DateTime.UtcNow;
                _db.Products.Add(product);
                await _db.SaveChangesAsync();

                if (!string.IsNullOrEmpty(filePath))
                    using (var stream = new FileStream("wwwroot/" + filePath, FileMode.Create))
                        await productDto.Image.CopyToAsync(stream);

                _response.IsSuccess = true;
                _response.Result = product;
                _response.Message = "Продукт добавлен.";
            }

            return _response;
        }

        public async Task<Response> DeleteProductAsync(int productId, ClaimsPrincipal User)
        {
            var product = await _db.Products.FindAsync(productId);

            if (User.IsInRole("StockLevelWorker")
                && product.StockId != Convert.ToInt32(User.FindFirstValue("StockId"))
            || User.IsInRole("CompanyLevelWorker")
                && product?.CompanyId != Convert.ToInt32(User.FindFirstValue("CompanyId"))
                )
                throw new Exception("Некорректные данные запроса");

            
            _response.IsSuccess = false;
            _response.Message = "Продукт не найден.";

            if (product != null)
            {
                _db.Products.Remove(product);

                if (File.Exists("wwwroot//" + product.ImagePath))
                    File.Delete("wwwroot//" + product.ImagePath);

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

        public async Task<Response> GetProductByIdAsync(int productId, ClaimsPrincipal User)
        {
            var product = await _db.Products.FindAsync(productId);

            if (User.IsInRole("StockLevelWorker")
                && product?.StockId != Convert.ToInt32(User.FindFirstValue("StockId"))
            || User.IsInRole("CompanyLevelWorker")
                && product?.CompanyId != Convert.ToInt32(User.FindFirstValue("CompanyId"))
                )
                throw new Exception("Некорректные данные запроса");

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

        public async Task<Response> UpdateProductAsync(ProductDto productDto, ClaimsPrincipal User)
        {
            var product = await _db.Products.FindAsync(productDto.ProductId);

            if (User.IsInRole("StockLevelWorker")
                && product.StockId != Convert.ToInt32(User.FindFirstValue("StockId"))
            || User.IsInRole("CompanyLevelWorker")
                && product?.CompanyId != Convert.ToInt32(User.FindFirstValue("CompanyId"))
                )
                throw new Exception("Некорректные данные запроса");

            _response.IsSuccess = false;
            _response.Message = "Продукт не найден.";

            if (product != null)
            {
                #region
                //if (productDto.ProductCategoryId != null)
                //{
                //    var pc = await _db.ProductCategories.FindAsync(productDto.ProductCategoryId);
                //    if (pc != null && pc.CompanyId == product.CompanyId) product.ProductCategoryId = productDto.ProductCategoryId;
                //}
                //if (productDto.UpdId != null)
                //{
                //    var upd = await _db.Upds.FindAsync(productDto.UpdId);
                //    if (upd != null && upd.CompanyId == product.CompanyId) product.UpdId = productDto.UpdId;
                //}
                //if (productDto.ShelfCode != null)
                //{
                //    var productIsExists = await _db.Products.AnyAsync(p => p.StorageLocationId == productDto.StorageLocationId
                //        && p.ShelfCode == productDto.ShelfCode);
                //    if (!productIsExists) product.ShelfCode = productDto.ShelfCode;
                //    else throw new Exception("Место хранения уже используется.");
                //}
                #endregion

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


                string filePath = "";
                string? oldPath = product.ImagePath;
                if (productDto.Image != null && productDto.Image.Length > 0)
                {
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(productDto.Image.FileName)}";
                    filePath = Path.Combine(_imagePath, fileName);
                    product.ImagePath = filePath;
                }


                await _db.SaveChangesAsync();

                if (!string.IsNullOrEmpty(filePath))
                {
                    using (var stream = new FileStream("wwwroot/" + filePath, FileMode.Create))
                        await productDto.Image.CopyToAsync(stream);

                    if (!string.IsNullOrEmpty(oldPath)
                        && File.Exists("wwwroot//" + oldPath))
                        File.Delete("wwwroot//" + oldPath);
                }

  
                _response.IsSuccess = true;
                _response.Result = product;
                _response.Message = "Данные продукта обновлены.";
            }

            return _response;
        }

        public async Task<Response> GetProductsFilteredAsync(ProductDto productDto, ClaimsPrincipal User)
        {
            if (User.IsInRole("StockLevelWorker")
                && (productDto.StockId = Convert.ToInt32(User.FindFirstValue("StockId"))) == -1
            || User.IsInRole("CompanyLevelWorker")
                && (productDto.CompanyId = Convert.ToInt32(User.FindFirstValue("CompanyId"))) == -1
                )
                throw new Exception("Некорректные данные запроса");

            _response.IsSuccess = false;
            _response.Message = "Продукты не найдены по указанным критериям.";

            var query = _db.Products.AsQueryable();

            if (productDto.ProductId != null)
                query = query.Where(p => p.ProductId == productDto.ProductId);

            if (!string.IsNullOrEmpty(productDto.RackCode))
                query = query.Where(p => p.RackCode == productDto.RackCode);
            if (!string.IsNullOrEmpty(productDto.ShelfCode))
                query = query.Where(p => p.ShelfCode == productDto.ShelfCode);

            if (productDto.LowerPriceLimit != null)
                query = query.Where(p => p.Price >= productDto.LowerPriceLimit);
            if (productDto.UpperPriceLimit != null)
                query = query.Where(p => p.Price <= productDto.UpperPriceLimit);

            if (!string.IsNullOrEmpty(productDto.Name))
                query = query.Where(p => EF.Functions.ILike(p.Name, $"%{productDto.Name}%"));
            if (!string.IsNullOrEmpty(productDto.Manufacturer))
                query = query.Where(p => EF.Functions.ILike(p.Manufacturer, $"%{productDto.Manufacturer}%"));
            if (!string.IsNullOrEmpty(productDto.ProductionArticle))
                query = query.Where(p => EF.Functions.ILike(p.ProductionArticle, $"%{productDto.ProductionArticle}%"));
            if (!string.IsNullOrEmpty(productDto.InnerArticle))
                query = query.Where(p => EF.Functions.ILike(p.InnerArticle, $"%{productDto.InnerArticle}%"));
            if (!string.IsNullOrEmpty(productDto.FactoryNumber))
                query = query.Where(p => EF.Functions.ILike(p.FactoryNumber, $"%{productDto.FactoryNumber}%"));


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
                query = query.Where(p => p.StockId == productDto.StockId);
            else if(productDto.CompanyId != null)
                query = query.Where(p => p.CompanyId == productDto.CompanyId);


            if (productDto.UpdId != null)
                query = query.Where(p => p.UpdId == productDto.UpdId);
            else if (productDto.BillId != null)
                query = query.Where(p => p.BillId == productDto.BillId);
            else if (productDto.ProviderId != null)
                query = query.Where(p => p.ProviderId == productDto.ProviderId);

            //var products = await query.ToListAsync();

            #region
            var products = await query
                .Select(p => new 
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Manufacturer = p.Manufacturer,
                    ProductionArticle = p.ProductionArticle,
                    InnerArticle = p.InnerArticle,
                    FactoryNumber = p.FactoryNumber,
                    Price = p.Price,
                    ImagePath = p.ImagePath,
                    CreateDate = p.CreateDate,

                    UpdId = p.UpdId,
                    DocumentNumber = p.UpdId != null ? _db.Upds
                        .Where(u => u.UpdId == p.UpdId)
                        .Select(u => u.DocumentNumber)
                        .FirstOrDefault() : null,

                    ProductCategoryId = p.ProductCategoryId,
                    ProductCategoryName = p.ProductCategoryId != null ? _db.ProductCategories
                        .Where(pc => pc.ProductCategoryId == p.ProductCategoryId)
                        .Select(pc => pc.Name)
                        .FirstOrDefault() : null,

                    StorageLocationId = p.StorageLocationId,
                    RackCode = p.RackCode,
                    ShelfCode = p.ShelfCode,

                    EmployeeId = p.EmployeeId,
                    EmployeeName = p.EmployeeId != null ? _db.Employees
                        .Where(e => e.EmployeeId == p.EmployeeId)
                        .Select(e => e.FullName)
                        .FirstOrDefault() : null,

                    StockId = p.StockId,
                    StockName = _db.Stocks
                        .Where(s => s.StockId == p.StockId)
                        .Select(s => s.Name)
                        .FirstOrDefault(),

                    CompanyId = p.CompanyId,
                    CompanyName = _db.Companies
                        .Where(c => c.CompanyId == p.CompanyId)
                        .Select(c => c.Name)
                        .FirstOrDefault(),

                    BillId = p.BillId,
                    BillNumber = p.BillId != null ? _db.Bills
                        .Where(b => b.BillId == p.BillId)
                        .Select(b => b.BillNumber)
                        .FirstOrDefault() : null,

                    ProviderId = p.ProviderId,
                    ProviderName = p.ProviderId != null ? _db.Providers
                        .Where(pr => pr.ProviderId == p.ProviderId)
                        .Select(pr => pr.Name)
                        .FirstOrDefault() : null
                })
                .ToListAsync();
            #endregion

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
