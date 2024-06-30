using StockService.Models.dto;
using StockService.Models;

namespace StockService.Repository.StorageLocationRep
{
    public interface IStorageLocationService
    {
        Task<Response> GetStorageLocationsByStockIdAsync(int stockId);
        Task<Response> GetStorageLocationsFilteredAsync(StorageLocationDto storageLocationDto);
        Task<Response> GetStorageLocationByIdAsync(int StorageLocationId);
        Task<Response> CreateStorageLocationAsync(StorageLocationDto storageLocation);
        Task<Response> UpdateStorageLocationAsync(StorageLocationDto storageLocation);
        Task<Response> DeleteStorageLocationAsync(int StorageLocationId);
    }
}
