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
            Response response = new Response();
            var companyIsExists = await _db.Companies.AnyAsync(c => c.Name == companyDto.Name);

            response.IsSuccess = false;
            response.Message = "Компания уже существует.";
            if (!companyIsExists)
            {
                var company = _mapper.Map<CompanyDto, Company>(companyDto);
                _db.Companies.Add(company);
                await _db.SaveChangesAsync();

                response.IsSuccess = true;
                response.Result = company;
                response.Message = "Компания добавлена.";
            }
            return response;
        }

        public async Task<Response> DeleteCompanyAsync(int id)
        {
            Response response = new Response();
            var company = await _db.Companies.FindAsync(id);

            response.IsSuccess = true;
            response.Message = "Компания не найдена.";

            if (company != null)
            {
                response.IsSuccess = false;
                response.Message = "Компания успешно удалена.";

                _db.Companies.Remove(company);
                await _db.SaveChangesAsync();
            }

            return response;
        }

        public async Task<Response> GetAllCompaniesAsync()
        {
            Response response = new Response();

            try
            {
                var companies = await _db.Companies.ToListAsync();
                response.IsSuccess = false;
                response.Message = "Компании не найдены.";

                if (companies.Count != 0)
                {
                    response.IsSuccess = true;
                    response.Result = companies;
                    response.Message = "Компании успешно получены.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors.Add(ex.Message);
            }

            return response;
        }

        public async Task<Response> GetCompanyByIdAsync(int id)
        {
            Response response = new Response();
            var company = await _db.Companies.FirstOrDefaultAsync(c => c.CompanyId == id);

            response.IsSuccess = false;
            response.Message = "Компания не найдена.";
            if (company != null)
            {
                response.IsSuccess = true;
                response.Result = company;
                response.Message = "Компания найдена.";

            }

            return response;
        }

        public async Task<Response> UpdateCompanyAsync(int id, CompanyDto companyDto)
        {
            Response response = new Response();
            var company = await _db.Companies.FirstOrDefaultAsync(e => e.CompanyId == id);

            response.IsSuccess = false;
            response.Message = "Компания не найдена.";

            if (company != null)
            {
                if (!string.IsNullOrEmpty(companyDto.Name))
                    company.Name = companyDto.Name;

                if (!string.IsNullOrEmpty(companyDto.INN))
                    company.INN = companyDto.INN;

                if (!string.IsNullOrEmpty(companyDto.Logo))
                    company.Logo = companyDto.Logo;


                await _db.SaveChangesAsync();

                response.IsSuccess = true;
                response.Result = company;
                response.Message = "Данные сотрудника обновлены.";
            }

            return response;
        }

    }
}
