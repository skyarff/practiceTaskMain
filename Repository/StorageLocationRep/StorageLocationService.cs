﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockService.Models;
using StockService.Models.dto;
using AppSettings;
using System.Text.RegularExpressions;

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
                    var filePath = $"{_imagePath}/{fileName}";


                    using (var stream = new FileStream("wwwroot/" + filePath, FileMode.Create))
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

        public async Task<Response> DeleteStorageLocationAsync(int storageLocationId)
        {
            var storageLocation = await _db.StorageLocations.FindAsync(storageLocationId);

            _response.IsSuccess = false;
            _response.Message = "Место хранения не найдено.";

            if (storageLocation != null)
            {
                _db.StorageLocations.Remove(storageLocation);
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Message = "Место хранения успешно удалено.";
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
                    var filePath = $"{_imagePath}/{fileName}";


                    using (var stream = new FileStream("wwwroot/" + filePath, FileMode.Create))
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

        public async Task<Response> GetStorageLocationsFilteredAsync(StorageLocationDto storageLocationDto)
        {
            _response.IsSuccess = false;
            _response.Message = "Места храненения не найдены по указанным критериям.";

            var query = _db.StorageLocations.AsQueryable();

            if (storageLocationDto.StockId != null)
                query = query.Where(sl => sl.StockId == storageLocationDto.StockId);
            if (storageLocationDto.IsBusy != null)
                query = query.Where(sl => (sl.Product != null) == (bool)storageLocationDto.IsBusy);

            if (!string.IsNullOrEmpty(storageLocationDto.Description))
                query = query.Where(sl => Regex.IsMatch(sl.Description, Regex.Escape(storageLocationDto.Description), RegexOptions.IgnoreCase));
            if (!string.IsNullOrEmpty(storageLocationDto.RackCode))
                query = query.Where(sl => sl.RackCode == storageLocationDto.RackCode);
            if (!string.IsNullOrEmpty(storageLocationDto.ShelfCode))
                query = query.Where(sl => sl.ShelfCode == storageLocationDto.ShelfCode);

            var storageLocations = await query.ToListAsync();


            if (storageLocations.Any())
            {
                _response.IsSuccess = true;
                _response.Result = storageLocations;
                _response.Message = "Места хранения успешно найдены по указанным критериям.";
            }

            return _response;
        }
    }
}
