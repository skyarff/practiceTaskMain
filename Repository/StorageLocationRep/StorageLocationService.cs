using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockService.Models;
using StockService.Models.dto;

namespace StockService.Repository.StorageLocationRep
{
    public class StorageLocationService : IStorageLocationService
    {
        private readonly StockContext _db;
        private readonly IMapper _mapper;
        private Response _response;
        public StorageLocationService(IMapper mapper, StockContext db)
        {
            _db = db;
            _mapper = mapper;
            _response = new Response();
        }
        
        public async Task<Response> CreateStorageLocationAsync(StorageLocationDto storageLocationDto)
        {
            var storageLocationIsExists = await _db.StorageLocations.AnyAsync(sl => 
            sl.StockId == storageLocationDto.StockId
            && sl.RackCode == storageLocationDto.RackCode
            && sl.ShelfCode == storageLocationDto.ShelfCode);

            _response.IsSuccess = false;
            _response.Message = "Место хранения уже существует.";

            if (!storageLocationIsExists)
            {
                var storageLocation = _mapper.Map<StorageLocationDto, StorageLocation>(storageLocationDto);

                if (storageLocationDto.Image != null && storageLocationDto.Image.Length > 0)
                {
                    var fileName = Path.GetFileName(storageLocationDto.Image.FileName);
                    var filePath = Path.Combine("\\Images\\StorageLocationImages", fileName);


                    using (var stream = new FileStream(Path.Combine("wwwroot\\Images\\StorageLocationImages", fileName), FileMode.Create))
                    {
                        await storageLocationDto.Image.CopyToAsync(stream);
                    }

                    storageLocation.ImagePath = filePath;
                }

                _db.StorageLocations.Add(storageLocation);
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Result = storageLocation;
                _response.Message = "Место хранения добавлено.";
            }

            return _response;
        }

        public async Task<Response> DeletesStorageLocationAsync(int storageLocationId)
        {
            var storageLocation = await _db.StorageLocations.FindAsync(storageLocationId);

            _response.IsSuccess = true;
            _response.Message = "Место хранения не найдено.";

            if (storageLocation != null)
            {
                _response.IsSuccess = false;
                _response.Message = "Место хранения успешно удалено.";

                _db.StorageLocations.Remove(storageLocation);
                await _db.SaveChangesAsync();
            }

            return _response;
        }

        public async Task<Response> GetStorageLocationsByStockIdAsync(int stockId)
        {
            _response.IsSuccess = false;
            _response.Message = "Места хранения не найдены.";

            var storageLocations = await _db.StorageLocations
                    .Where(sl => sl.StockId == stockId)
                    .ToListAsync();

            if (storageLocations.Any())
            {
                _response.IsSuccess = true;
                _response.Result = storageLocations;
                _response.Message = $"Места хранения для склада с ID {stockId} успешно получены.";
            }

            return _response;
        }

        public async Task<Response> GetStorageLocationByIdAsync(int storageLocationId)
        {
            var storageLocation = await _db.StorageLocations.FindAsync(storageLocationId);

            _response.IsSuccess = false;
            _response.Message = "Место хранения не найдено.";
            if (storageLocation != null)
            {
                _response.IsSuccess = true;
                _response.Result = storageLocation;
                _response.Message = "Место хранения найдено.";
            }

            return _response;
        }

        public async Task<Response> UpdateStorageLocationAsync(StorageLocationDto storageLocationDto)
        {
            var storageLocation = await _db.StorageLocations.FindAsync(storageLocationDto.StorageLocationId);

            _response.IsSuccess = false;
            _response.Message = "Место хранения не найдено.";

            if (storageLocation != null)
            {
                if (!string.IsNullOrEmpty(storageLocationDto.Description))
                    storageLocation.Description = storageLocationDto.Description;

                if (storageLocationDto.Image != null && storageLocationDto.Image.Length > 0)
                {
                    if (!string.IsNullOrEmpty(storageLocation.ImagePath))
                    {
                        var oldImagePath = Path.Combine("wwwroot", storageLocation.ImagePath.TrimStart('\\'));
                        if (File.Exists(oldImagePath))
                        {
                            File.Delete(oldImagePath);
                        }
                    }

                    var fileName = Path.GetFileName(storageLocationDto.Image.FileName);
                    var filePath = Path.Combine("\\Images\\EmployeeImages", fileName);

                    using (var stream = new FileStream(Path.Combine("wwwroot\\Images\\EmployeeImages", fileName), FileMode.Create))
                    {
                        await storageLocationDto.Image.CopyToAsync(stream);
                    }
                    storageLocation.ImagePath = filePath;
                }

                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Result = storageLocation;
                _response.Message = "Данные места хранения обновлены.";
            }

            return _response;
        }

    }
}
