using AppSettings;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockService.Models;
using StockService.Models.dto;
using System.Security.Claims;

namespace StockService.Repository.StorageLocationRep
{
    public class StorageLocationService : IStorageLocationService
    {
        private readonly StockContext _db;
        private readonly IMapper _mapper;
        private Response _response;
        private string _imagePath = PathSettings.ImagePaths["StorageLocationImages"];
        public StorageLocationService(IMapper mapper, StockContext db)
        {
            _db = db;
            _mapper = mapper;
            _response = new Response();
        }
        
        public async Task<Response> CreateStorageLocationAsync(StorageLocationDto storageLocationDto, ClaimsPrincipal User)
        {
            var stock = await _db.Stocks.FindAsync(storageLocationDto.StockId);

            if (User.IsInRole("StockLevelWorker")
                && storageLocationDto.StockId != Convert.ToInt32(User.FindFirstValue("StockId"))
                || User.IsInRole("CompanyLevelWorker")
                && stock?.CompanyId != Convert.ToInt32(User.FindFirstValue("CompanyId"))
                )
                throw new Exception("Некорректные данные запроса");

            var storageLocationIsExists = await _db.StorageLocations.AnyAsync(sl => 
            sl.StockId == storageLocationDto.StockId
            && sl.RackCode == storageLocationDto.RackCode);

            _response.IsSuccess = false;
            _response.Message = "Стеллаж уже существует.";

            if (!storageLocationIsExists)
            {
                var storageLocation = _mapper.Map<StorageLocationDto, StorageLocation>(storageLocationDto);
                storageLocation.CompanyId = stock.CompanyId;

                string filePath = "";
                if (storageLocationDto.Image != null && storageLocationDto.Image.Length > 0)
                {
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(storageLocationDto.Image.FileName)}";
                    filePath = Path.Combine(_imagePath, fileName);
                    storageLocation.ImagePath = filePath;
                }

                _db.StorageLocations.Add(storageLocation);
                await _db.SaveChangesAsync();

                if (!string.IsNullOrEmpty(filePath))
                    using (var stream = new FileStream("wwwroot/" + filePath, FileMode.Create))
                        await storageLocationDto.Image.CopyToAsync(stream);

                _response.IsSuccess = true;
                _response.Result = storageLocation;
                _response.Message = "Место хранения добавлено.";
            }

            return _response;
        }

        public async Task<Response> DeleteStorageLocationAsync(int storageLocationId, ClaimsPrincipal User)
        {
            var storageLocation = await _db.StorageLocations.FindAsync(storageLocationId);

            if (User.IsInRole("StockLevelWorker")
                && storageLocation?.StockId != Convert.ToInt32(User.FindFirstValue("StockId"))
                || User.IsInRole("CompanyLevelWorker")
                && storageLocation?.CompanyId != Convert.ToInt32(User.FindFirstValue("CompanyId"))
                )
                throw new Exception("Некорректные данные запроса");

            

            _response.IsSuccess = false;
            _response.Message = "Стеллаж не найден.";

            if (storageLocation != null)
            {
                _db.StorageLocations.Remove(storageLocation);

                if (File.Exists("wwwroot//" + storageLocation.ImagePath))
                    File.Delete("wwwroot//" + storageLocation.ImagePath);

                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Message = "Стеллаж успешно удален.";
            }

            return _response;
        }

        public async Task<Response> GetStorageLocationsByStockIdAsync(int stockId, ClaimsPrincipal User)
        {
            var stock = await _db.Stocks.FindAsync(stockId);

            if (User.IsInRole("StockLevelWorker")
                && stockId != Convert.ToInt32(User.FindFirstValue("StockId"))
                || User.IsInRole("CompanyLevelWorker")
                && stock?.CompanyId != Convert.ToInt32(User.FindFirstValue("CompanyId"))
                )
                throw new Exception("Некорректные данные запроса");

            _response.IsSuccess = false;
            _response.Message = "Стеллажы не найдены.";

            var storageLocations = await _db.StorageLocations
                    .Where(sl => sl.StockId == stockId)
                    .ToListAsync();

            if (storageLocations.Any())
            {
                _response.IsSuccess = true;
                _response.Result = storageLocations;
                _response.Message = $"Стеллажы для склада с ID {stockId} успешно получены.";
            }

            return _response;
        }

        public async Task<Response> GetStorageLocationByIdAsync(int storageLocationId, ClaimsPrincipal User)
        {
            var storageLocation = await _db.StorageLocations.FindAsync(storageLocationId);

            if (User.IsInRole("StockLevelWorker")
                && storageLocation?.StockId != Convert.ToInt32(User.FindFirstValue("StockId"))
                || User.IsInRole("CompanyLevelWorker")
                && storageLocation?.CompanyId != Convert.ToInt32(User.FindFirstValue("CompanyId"))
                )
                throw new Exception("Некорректные данные запроса");

            _response.IsSuccess = false;
            _response.Message = "Стеллаж не найден.";
            if (storageLocation != null)
            {
                _response.IsSuccess = true;
                _response.Result = storageLocation;
                _response.Message = "Стеллаж найден.";
            }

            return _response;
        }

        public async Task<Response> UpdateStorageLocationAsync(StorageLocationDto storageLocationDto, ClaimsPrincipal User)
        {
            var storageLocation = await _db.StorageLocations.FindAsync(storageLocationDto.StorageLocationId);

            if (User.IsInRole("StockLevelWorker")
                && storageLocation?.StockId != Convert.ToInt32(User.FindFirstValue("StockId"))
                || User.IsInRole("CompanyLevelWorker")
                && storageLocation?.CompanyId != Convert.ToInt32(User.FindFirstValue("CompanyId"))
                )
                throw new Exception("Некорректные данные запроса");

            _response.IsSuccess = false;
            _response.Message = "Стеллаж не найден.";

            if (storageLocation != null)
            {
                if (!string.IsNullOrEmpty(storageLocationDto.Description))
                    storageLocation.Description = storageLocationDto.Description;

                string filePath = "";
                string? oldPath = storageLocation.ImagePath;
                if (storageLocationDto.Image != null && storageLocationDto.Image.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(storageLocationDto.Image.FileName)}";
                    filePath = Path.Combine(_imagePath, fileName);
                    storageLocation.ImagePath = filePath;
                }

                await _db.SaveChangesAsync();

                if (!string.IsNullOrEmpty(filePath))
                {
                    using (var stream = new FileStream("wwwroot/" + filePath, FileMode.Create))
                        await storageLocationDto.Image.CopyToAsync(stream);

                    if (!string.IsNullOrEmpty(oldPath)
                        && File.Exists("wwwroot//" + oldPath))
                            File.Delete("wwwroot//" + oldPath);
                }


                _response.IsSuccess = true;
                _response.Result = storageLocation;
                _response.Message = "Данные стеллажа обновлены.";
            }

            return _response;
        }

        public async Task<Response> GetStorageLocationsFilteredAsync(StorageLocationDto storageLocationDto, ClaimsPrincipal User)
        {
            var storageLocation = await _db.StorageLocations.FindAsync(storageLocationDto.StorageLocationId);

            if (User.IsInRole("StockLevelWorker")
                && (storageLocationDto.StockId = Convert.ToInt32(User.FindFirstValue("StockId"))) == -1
                || User.IsInRole("CompanyLevelWorker")
                && (storageLocationDto.CompanyId = Convert.ToInt32(User.FindFirstValue("CompanyId"))) == -1
                )
                throw new Exception("Некорректные данные запроса");

            _response.IsSuccess = false;
            _response.Message = "Стеллажы не найдены по указанным критериям.";

            var query = _db.StorageLocations.AsQueryable();

            if (storageLocationDto.StorageLocationId != null)
                query = query.Where(sl => sl.StorageLocationId == storageLocationDto.StorageLocationId);

            if (storageLocationDto.StockId != null)
                query = query.Where(sl => sl.StockId == storageLocationDto.StockId);
            if (storageLocationDto.CompanyId != null)
                query = query.Where(sl => sl.CompanyId == storageLocationDto.CompanyId);


            if (!string.IsNullOrEmpty(storageLocationDto.Description))
                query = query.Where(sl => EF.Functions.ILike(sl.Description, $"%{storageLocationDto.Description}%"));

            if (!string.IsNullOrEmpty(storageLocationDto.RackCode))
                query = query.Where(sl => EF.Functions.ILike(sl.RackCode, $"%{storageLocationDto.RackCode}%"));


            //var storageLocations = await query.ToListAsync();

            #region
            var storageLocations = await query
                .Select(sl => new
                {
                    StorageLocationId = sl.StorageLocationId,
                    RackCode = sl.RackCode,
                    Description = sl.Description,
                    ImagePath = sl.ImagePath,

                    StockId = sl.StockId,
                    StockName = _db.Stocks
                        .Where(s => s.StockId == sl.StockId)
                        .Select(s => s.Name)
                        .FirstOrDefault(),

                    CompanyId = sl.CompanyId,
                    CompanyName = _db.Companies
                        .Where(c => c.CompanyId == sl.CompanyId)
                        .Select(c => c.Name)
                        .FirstOrDefault(),
                })
                .ToListAsync();
            #endregion


            if (storageLocations.Any())
            {
                _response.IsSuccess = true;
                _response.Result = storageLocations;
                _response.Message = "Стеллажы успешно найдены по указанным критериям.";
            }

            return _response;
        }
    }
}
