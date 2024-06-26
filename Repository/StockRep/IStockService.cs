﻿using StockService.Models.dto;
using StockService.Models;

namespace StockService.Repository.StockRep
{
    public interface IStockService
    {
        Task<Response> GetAllStocksAsync();
        Task<Response> GetStockByIdAsync(int id);
        Task<Response> CreateStockAsync(StockDto stockDto);
        Task<Response> UpdateStockAsync(int id, StockDto stockDto);
        Task<Response> DeleteStockAsync(int id);
    }
}
