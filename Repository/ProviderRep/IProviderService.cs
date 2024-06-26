﻿using StockService.Models;
using StockService.Models.dto;

namespace StockService.Repository.ProviderRep
{
    public interface IProviderService
    {
        Task<Response> GetAllProvidersAsync();
        Task<Response> GetProviderByIdAsync(int providerId);
        Task<Response> CreateProviderAsync(ProviderDto providerId);
        Task<Response> UpdateProviderAsync(ProviderDto providerDto);
        Task<Response> DeleteProviderAsync(int iproviderId);
        Task<Response> GetProvidersFilteredAsync(ProviderDto providerDto);
    }
}
