using StockService.Models.dto;
using StockService.Models;
using System.Security.Claims;

namespace StockService.Repository.StorageLocationRep
{
    public interface IStorageLocationService
    {
        Task<Response> GetStorageLocationsByStockIdAsync(int stockId, ClaimsPrincipal User);
        Task<Response> GetStorageLocationsFilteredAsync(StorageLocationDto storageLocationDto, ClaimsPrincipal User);
        Task<Response> GetStorageLocationByIdAsync(int StorageLocationId, ClaimsPrincipal User);
        Task<Response> CreateStorageLocationAsync(StorageLocationDto storageLocation, ClaimsPrincipal User);
        Task<Response> UpdateStorageLocationAsync(StorageLocationDto storageLocation, ClaimsPrincipal User);
        Task<Response> DeleteStorageLocationAsync(int StorageLocationId, ClaimsPrincipal User);
    }
}
