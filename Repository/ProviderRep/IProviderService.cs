﻿using StockService.Models.dto;
using StockService.Models;

namespace StockService.Repository.ProviderRep
{
    public interface IProviderService
    {
        Task<Response> GetAllProvidersAsync();
        Task<Response> GetProviderByIdAsync(int id);
        Task<Response> CreateProviderAsync(ProviderDto providerDto);
        Task<Response> UpdateProviderAsync(int id, ProviderDto providerDto);
        Task<Response> DeleteProviderAsync(int id);
    }
}