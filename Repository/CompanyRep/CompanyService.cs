using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockService.Models;
using StockService.Models.dto;

namespace StockService.Repository.CompanyRep
{
    public class CompanyService : ICompanyService
    {
        private readonly StockContext _db;
        private readonly IMapper _mapper;
        private Response _response;

        public CompanyService(IMapper mapper, StockContext db)
        {
            _db = db;
            _mapper = mapper;
            _response = new Response();
        }

        public async Task<Response> CreateCompanyAsync(CompanyDto companyDto)
        {
            var companyIsExists = await _db.Companies.AnyAsync(c => c.Name == companyDto.Name);

            _response.IsSuccess = false;
            _response.Message = "Компания уже существует.";
            if (!companyIsExists)
            {
                var company = _mapper.Map<CompanyDto, Company>(companyDto);
                _db.Companies.Add(company);
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Result = company;
                _response.Message = "Компания добавлена.";
            }
            return _response;
        }

        public async Task<Response> DeleteCompanyAsync(int companyId)
        {
            var company = await _db.Companies.FindAsync(companyId);

            _response.IsSuccess = false;
            _response.Message = "Компания не найдена.";

            if (company != null)
            {
                _db.Companies.Remove(company);
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Message = "Компания успешно удалена.";
            }

            return _response;
        }

        public async Task<Response> GetAllCompaniesAsync()
        {

            var companies = await _db.Companies.ToListAsync();
            _response.IsSuccess = false;
            _response.Message = "Компании не найдены.";

            if (companies.Any())
            {
                _response.IsSuccess = true;
                _response.Result = companies;
                _response.Message = "Компании получены.";
            }

            return _response;
        }

        public async Task<Response> GetCompanyByIdAsync(int companyId)
        {
            var company = await _db.Companies.FindAsync(companyId);

            _response.IsSuccess = false;
            _response.Message = "Компания не найдена.";
            if (company != null)
            {
                _response.IsSuccess = true;
                _response.Result = company;
                _response.Message = "Компания найдена.";
            }

            return _response;
        }

        public async Task<Response> UpdateCompanyAsync(CompanyDto companyDto)
        {
            var company = await _db.Companies.FindAsync(companyDto.CompanyId);

            _response.IsSuccess = false;
            _response.Message = "Компания не найдена.";

            if (company != null)
            {
                if (!string.IsNullOrEmpty(companyDto.Name))
                    company.Name = companyDto.Name;

                if (!string.IsNullOrEmpty(companyDto.INN))
                    company.INN = companyDto.INN;

                if (!string.IsNullOrEmpty(companyDto.Logo))
                    company.Logo = companyDto.Logo;


                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Result = company;
                _response.Message = "Данные компании обновлены.";
            }

            return _response;
        }

    }
}
