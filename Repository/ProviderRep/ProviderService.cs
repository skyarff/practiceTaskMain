using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockService.Models;
using StockService.Models.dto;

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
            Response response = new Response();
            var providerIsExists = await _db.Providers.AnyAsync(p => p.Name == providerDto.Name);

            response.IsSuccess = false;
            response.Message = "Поставщик уже существует.";
            if (!providerIsExists)
            {
                var provider = _mapper.Map<ProviderDto, Provider>(providerDto);
                _db.Providers.Add(provider);


                await _db.SaveChangesAsync();

                response.IsSuccess = true;
                response.Result = provider;
                response.Message = "Поставщик добавлен.";
            }
            return response;
        }

        public async Task<Response> DeleteProviderAsync(int id)
        {
            Response response = new Response();
            var provider = await _db.Providers.FindAsync(id);

            response.IsSuccess = true;
            response.Message = "Поставщик не найден.";

            if (provider != null)
            {
                response.IsSuccess = false;
                response.Message = "Поставщик успешно удален.";

                _db.Providers.Remove(provider);
                await _db.SaveChangesAsync();
            }

            return response;
        }

        public async Task<Response> GetAllProvidersAsync()
        {
            Response response = new Response();

            var providers = await _db.Providers.ToListAsync();
            response.IsSuccess = false;
            response.Message = "Поставщики не найдены.";

            if (providers.Count != 0)
            {
                response.IsSuccess = true;
                response.Result = providers;
                response.Message = "Поставщики успешно получены.";
            }

            return response;
        }

        public async Task<Response> GetProviderByIdAsync(int id)
        {
            
            var provider = await _db.Providers.FirstOrDefaultAsync(p => p.ProviderId == id);

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

        public async Task<Response> UpdateProviderAsync(int id, ProviderDto providerDto)
        {
            Response response = new Response();
            var provider = await _db.Providers.FirstOrDefaultAsync(p => p.ProviderId == id);

            response.IsSuccess = false;
            response.Message = "Поставщик не найден.";

            if (provider != null)
            {
                if (!string.IsNullOrEmpty(providerDto.Name))
                    provider.Name = providerDto.Name;

                if (!string.IsNullOrEmpty(providerDto.INN))
                    provider.INN = providerDto.INN;

                if (!string.IsNullOrEmpty(providerDto.LegalAdress))
                    provider.LegalAdress = providerDto.LegalAdress;

                if (!string.IsNullOrEmpty(providerDto.CheckingAccount))
                    provider.CheckingAccount = providerDto.CheckingAccount;

                if (!string.IsNullOrEmpty(providerDto.Bank))
                    provider.Bank = providerDto.Bank;

                if (!string.IsNullOrEmpty(providerDto.BIK))
                    provider.BIK = providerDto.BIK;

                if (!string.IsNullOrEmpty(providerDto.CorrAccount))
                    provider.CorrespondentAccount = providerDto.CorrAccount;

                if (!string.IsNullOrEmpty(providerDto.ManagerFullname))
                    provider.ManagerFullname = providerDto.ManagerFullname;

                if (!string.IsNullOrEmpty(providerDto.Email))
                    provider.Email = providerDto.Email;

                if (!string.IsNullOrEmpty(providerDto.Phone))
                    provider.Phone = providerDto.Phone;


                await _db.SaveChangesAsync();

                response.IsSuccess = true;
                response.Result = provider;
                response.Message = "Данные сотрудника обновлены.";
            }

            return response;
        }
    }
}
