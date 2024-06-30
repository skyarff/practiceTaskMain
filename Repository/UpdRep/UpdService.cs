using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockService.Models;
using StockService.Models.dto;
using StockService.Repository.UpdRep;
using AppSettings;

namespace StockService.Repository.BillRep
{
    public class UpdService : IUpdService
    {
        private readonly StockContext _db;
        private readonly IMapper _mapper;
        private Response _response;
        private string _imagePath = PathSettings.ImagePaths["UpdPdfs"];
        public UpdService(IMapper mapper, StockContext db)
        {
            _db = db;
            _mapper = mapper;
            _response = new Response();
        }

        public async Task<Response> CreateUpdAsync(UpdDto updDto)
        {
            var updIsExists = await _db.Upds.AnyAsync(u => u.DocumentNumber == updDto.DocumentNumber);

            _response.IsSuccess = false;
            _response.Message = "УПД уже существует.";

            if (!updIsExists)
            {
                var upd = _mapper.Map<UpdDto, Upd>(updDto);

                if (updDto.UpdPdf != null && updDto.UpdPdf.Length > 0)
                {

                    var fileName = Path.GetFileName(updDto.UpdPdf.FileName);
                    var filePath = $"{_imagePath}/{fileName}";


                    using (var stream = new FileStream("wwwroot/" + filePath, FileMode.Create))
                    {
                        await updDto.UpdPdf.CopyToAsync(stream);
                    }

                    upd.UpdPdfPath = filePath;
                }

                upd.CreateDate = DateTime.UtcNow;
                _db.Upds.Add(upd);
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Result = upd;
                _response.Message = "Место хранения добавлено добавлено.";
            }

            return _response;
        }

        public async Task<Response> DeleteUpdAsync(int updId)
        {
            var upd = await _db.Upds.FindAsync(updId);

            _response.IsSuccess = false;
            _response.Message = "УПД не найден.";

            if (upd != null)
            {
                _db.Upds.Remove(upd);
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Message = "УПД успешно удален.";
            }

            return _response;
        }

        public async Task<Response> GetAllUpdsAsync()
        {
            var upds = await _db.Upds.ToListAsync();

            _response.IsSuccess = false;
            _response.Message = "УПД не найдены.";

            if (upds.Any())
            {
                _response.IsSuccess = true;
                _response.Result = upds;
                _response.Message = "УПД успешно получены.";
            }

            return _response;
        }

        public async Task<Response> GetUpdsInRangeAsync(UpdDto updDto)
        {
            var query = _db.Upds.AsQueryable();

            if (updDto.StartDate != null)
                query = query.Where(b => b.CreateDate >= updDto.StartDate.Value);

            if (updDto.EndDate != null)
                query = query.Where(b => b.CreateDate <= updDto.EndDate.Value);

            bool ascending = updDto.Ascending == null ? true : (bool)updDto.Ascending;
            query = ascending
                ? query.OrderBy(b => b.CreateDate)
                : query.OrderByDescending(b => b.CreateDate);

            var upds = await query.ToListAsync();

            _response.IsSuccess = false;
            _response.Message = "УПД не найдены.";

            if (upds.Any())
            {
                _response.IsSuccess = true;
                _response.Result = upds;
                _response.Message = "УПД успешно получены.";
            }

            return _response;
        }

        public async Task<Response> GetUpdsByProviderIdAsync(int providerId)
        {
            _response.IsSuccess = false;
            _response.Message = "УПД не найдены для указанного поставщика.";

            var upds = await _db.Upds
                    .Where(u => u.ProviderId == providerId)
                    .ToListAsync();

            if (upds.Any())
            {
                _response.IsSuccess = true;
                _response.Result = upds;
                _response.Message = $"УПД от поставщика с ID {providerId} успешно получены.";
            }

            return _response;
        }

        public async Task<Response> GetUpdByIdAsync(int updId)
        {
            var upd = await _db.Upds.FindAsync(updId);

            _response.IsSuccess = false;
            _response.Message = "УПД не найден.";

            if (upd != null)
            {
                _response.IsSuccess = true;
                _response.Result = upd;
                _response.Message = "УПД найден.";

            }

            return _response;
        }
    }
}
