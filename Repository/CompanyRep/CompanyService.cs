using AppSettings;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockService.Models;
using StockService.Models.dto;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace StockService.Repository.CompanyRep
{
    public class CompanyService : ICompanyService
    {
        private readonly StockContext _db;
        private readonly IMapper _mapper;
        private Response _response;
        private string _imagePath = PathSettings.ImagePaths["CompanyLogos"];

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

                if (companyDto.Image != null && companyDto.Image.Length > 0)
                {
                    var fileName = Path.GetFileName(companyDto.Image.FileName);
                    var filePath = $"{_imagePath}/{fileName}";


                    using (var stream = new FileStream("wwwroot/" + filePath, FileMode.Create))
                    {
                        await companyDto.Image.CopyToAsync(stream);
                    }

                    company.LogoPath = filePath;
                }

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

                if (!string.IsNullOrEmpty(companyDto.Inn))
                    company.Inn = companyDto.Inn;

                if (companyDto.Image != null && companyDto.Image.Length > 0)
                {
                    if (!string.IsNullOrEmpty(company.LogoPath))
                    {
                        var oldImagePath = Path.Combine("wwwroot", company.LogoPath.TrimStart('\\'));
                        if (File.Exists(oldImagePath))
                        {
                            File.Delete(oldImagePath);
                        }
                    }

                    var fileName = Path.GetFileName(companyDto.Image.FileName);
                    var filePath = $"{_imagePath}/{fileName}";


                    using (var stream = new FileStream("wwwroot/" + filePath, FileMode.Create))
                    {
                        await companyDto.Image.CopyToAsync(stream);
                    }
                    company.LogoPath = filePath;
                }


                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Result = company;
                _response.Message = "Данные компании обновлены.";
            }

            return _response;
        }

        public async Task<Response> GetCompaniesFilteredAsync(CompanyDto companyDto)
        {
            _response.IsSuccess = false;
            _response.Message = "Компании не найдены по указанным критериям.";

            var query = _db.Companies.AsQueryable();


            if (!string.IsNullOrEmpty(companyDto.Name))
                query = query.Where(c => Regex.IsMatch(c.Name, Regex.Escape(companyDto.Name), RegexOptions.IgnoreCase));
            if (!string.IsNullOrEmpty(companyDto.Inn))
                query = query.Where(c => Regex.IsMatch(c.Inn, Regex.Escape(companyDto.Inn), RegexOptions.IgnoreCase));


            var companies = await query.ToListAsync();
            if (companies.Any())
            {
                _response.IsSuccess = true;
                _response.Result = companies;
                _response.Message = "Компании успешно найдены по указанным критериям.";
            }

            return _response;
        }
    }
}
