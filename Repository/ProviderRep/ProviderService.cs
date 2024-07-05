using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockService.Models;
using StockService.Models.dto;
using System.Text.RegularExpressions;

namespace StockService.Repository.ProviderRep
{
    public class ProviderService : IProviderService
    {
        private readonly StockContext _db;
        private readonly IMapper _mapper;
        private Response _response;

        public ProviderService(IMapper mapper, StockContext db)
        {
            _db = db;
            _mapper = mapper;
            _response = new Response();
        }

        public async Task<Response> CreateProviderAsync(ProviderDto providerDto)
        {
            var providerIsExists = await _db.Providers.AnyAsync(p => p.Name == providerDto.Name);

            _response.IsSuccess = false;
            _response.Message = "Поставщик уже существует.";
            if (!providerIsExists)
            {
                var provider = _mapper.Map<ProviderDto, Provider>(providerDto);
                _db.Providers.Add(provider);


                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Result = provider;
                _response.Message = "Поставщик добавлен.";
            }

            return _response;
        }

        public async Task<Response> DeleteProviderAsync(int providerId)
        {
            var provider = await _db.Providers.FindAsync(providerId);

            _response.IsSuccess = false;
            _response.Message = "Поставщик не найден.";

            if (provider != null)
            {
                _db.Providers.Remove(provider);
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Message = "Поставщик успешно удален.";
            }

            return _response;
        }

        public async Task<Response> GetAllProvidersAsync()
        {
            var providers = await _db.Providers.ToListAsync();
            _response.IsSuccess = false;
            _response.Message = "Поставщики не найдены.";

            if (providers.Any())
            {
                _response.IsSuccess = true;
                _response.Result = providers;
                _response.Message = "Поставщики успешно получены.";
            }

            return _response;
        }

        public async Task<Response> GetProviderByIdAsync(int providerId)
        {
            var provider = await _db.Providers.FindAsync(providerId);

            _response.IsSuccess = false;
            _response.Message = "Поставщик не найден.";
            if (provider != null)
            {
                _response.IsSuccess = true;
                _response.Result = provider;
                _response.Message = "Поставщик найден.";
            }

            return _response;
        }

        public async Task<Response> UpdateProviderAsync(ProviderDto providerDto)
        {
            var provider = await _db.Providers.FindAsync(providerDto.ProviderId);

            _response.IsSuccess = false;
            _response.Message = "Поставщик не найден.";

            if (provider != null)
            {
                if (!string.IsNullOrEmpty(providerDto.Name))
                    provider.Name = providerDto.Name;

                if (!string.IsNullOrEmpty(providerDto.Inn))
                    provider.Inn = providerDto.Inn;

                if (!string.IsNullOrEmpty(providerDto.LegalAdress))
                    provider.LegalAdress = providerDto.LegalAdress;

                if (!string.IsNullOrEmpty(providerDto.CheckingAccount))
                    provider.CheckingAccount = providerDto.CheckingAccount;

                if (!string.IsNullOrEmpty(providerDto.Bank))
                    provider.Bank = providerDto.Bank;

                if (!string.IsNullOrEmpty(providerDto.Bik))
                    provider.Bik = providerDto.Bik;

                if (!string.IsNullOrEmpty(providerDto.CorrespondentAccount))
                    provider.CorrespondentAccount = providerDto.CorrespondentAccount;

                if (!string.IsNullOrEmpty(providerDto.ManagerFullname))
                    provider.ManagerFullname = providerDto.ManagerFullname;

                if (!string.IsNullOrEmpty(providerDto.Email))
                    provider.Email = providerDto.Email;

                if (!string.IsNullOrEmpty(providerDto.Phone))
                    provider.Phone = providerDto.Phone;


                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Result = provider;
                _response.Message = "Данные поставщика обновлены.";
            }

            return _response;
        }

        public async Task<Response> GetProvidersFilteredAsync(ProviderDto providerDto)
        {
            _response.IsSuccess = false;
            _response.Message = "Поставщики не найдены по указанным критериям.";

            var query = _db.Providers.AsQueryable();

            if (providerDto.ProviderId != null)
                query = query.Where(p => p.ProviderId == providerDto.ProviderId);

            if (!string.IsNullOrEmpty(providerDto.Name))
                query = query.Where(p => EF.Functions.ILike(p.Name, $"%{providerDto.Name}%"));
            if (!string.IsNullOrEmpty(providerDto.Inn))
                query = query.Where(p => EF.Functions.ILike(p.Inn, $"%{providerDto.Inn}%"));
            if (!string.IsNullOrEmpty(providerDto.LegalAdress))
                query = query.Where(p => EF.Functions.ILike(p.LegalAdress, $"%{providerDto.LegalAdress}%"));
            if (!string.IsNullOrEmpty(providerDto.CheckingAccount))
                query = query.Where(p => EF.Functions.ILike(p.CheckingAccount, $"%{providerDto.CheckingAccount}%"));
            if (!string.IsNullOrEmpty(providerDto.Bank))
                query = query.Where(p => EF.Functions.ILike(p.Bank, $"%{providerDto.Bank}%"));
            if (!string.IsNullOrEmpty(providerDto.Bik))
                query = query.Where(p => EF.Functions.ILike(p.Bik, $"%{providerDto.Bik}%"));
            if (!string.IsNullOrEmpty(providerDto.CorrespondentAccount))
                query = query.Where(p => EF.Functions.ILike(p.CorrespondentAccount, $"%{providerDto.CorrespondentAccount}%"));
            if (!string.IsNullOrEmpty(providerDto.ManagerFullname))
                query = query.Where(p => EF.Functions.ILike(p.ManagerFullname, $"%{providerDto.ManagerFullname}%"));
            if (!string.IsNullOrEmpty(providerDto.Email))
                query = query.Where(p => EF.Functions.ILike(p.Email, $"%{providerDto.Email}%"));
            if (!string.IsNullOrEmpty(providerDto.Phone))
                query = query.Where(p => EF.Functions.ILike(p.Phone, $"%{providerDto.Phone}%"));

            var companies = await query.ToListAsync();
            if (companies.Any())
            {
                _response.IsSuccess = true;
                _response.Result = companies;
                _response.Message = "Поставщики успешно найдены по указанным критериям.";
            }

            return _response;
        }
    }
}
