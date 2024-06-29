using StockService.Models.dto;
using StockService.Models;

namespace StockService.Repository.StorageLocationRep
{
    public interface IStorageLocationService
    {
        Task<Response> GetStorageLocationsByStockIdAsync(int stockId);
        Task<Response> GetStorageLocationByIdAsync(int StorageLocationId);
        Task<Response> CreateStorageLocationAsync(StorageLocationDto storageLocation);
        Task<Response> UpdateStorageLocationAsync(int StorageLocationId, StorageLocationDto storageLocation);
        Task<Response> DeletesStorageLocationAsync(int StorageLocationId);
    }
}
