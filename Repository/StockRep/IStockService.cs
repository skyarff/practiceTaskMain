﻿using StockService.Models;
using StockService.Models.dto;

namespace StockService.Repository.StockRep
{
    public interface IStockService
    {
        Task<Response> GetAllStocksAsync();
        Task<Response> GetStocksByCompanyIdAsync(int? companyId);
        Task<Response> GetStockByIdAsync(int stockId);
        Task<Response> CreateStockAsync(StockDto stockDto);
        Task<Response> UpdateEmployeeAsync(StockDto stockDto);
        Task<Response> DeleteStockAsync(int stockId);
        Task<Response> GetStocksFilteredAsync(StockDto stockDto);
    }
}
