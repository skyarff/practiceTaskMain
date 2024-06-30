using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockService.Models;
using StockService.Models.dto;
using System.Linq;
using AppSettings;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace StockService.Repository.BillRep
{
    public class BillService : IBillService
    {
        private readonly StockContext _db;
        private readonly IMapper _mapper;
        private Response _response;
        private string _imagePath = PathSettings.ImagePaths["BillPdfs"];
        public BillService(IMapper mapper, StockContext db)
        {
            _db = db;
            _mapper = mapper;
            _response = new Response();
        }

        public async Task<Response> CreateBillAsync(BillDto billDto)
        {
            var billIsExists = await _db.Bills.AnyAsync(b => b.BillNumber == billDto.BillNumber);

            _response.IsSuccess = false;
            _response.Message = "Счет уже существует.";

            if (!billIsExists)
            {
                var bill = _mapper.Map<BillDto, Bill>(billDto);

                if (billDto.BillPdf != null && billDto.BillPdf.Length > 0)
                {
                    var fileName = Path.GetFileName(billDto.BillPdf.FileName);
                    var filePath = $"{_imagePath}/{fileName}";


                    using (var stream = new FileStream("wwwroot/" + filePath, FileMode.Create))
                    {
                        await billDto.BillPdf.CopyToAsync(stream);
                    }

                    bill.BillPdfPath = filePath;
                }

                bill.CreateDate = DateTime.UtcNow;
                _db.Bills.Add(bill);
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Result = bill;
                _response.Message = "Счет добавлен.";
            }

            return _response;
        }

        public async Task<Response> DeleteBillAsync(int billId)
        {
            var bill = await _db.Bills.FindAsync(billId);

            _response.IsSuccess = false;
            _response.Message = "Счет не найден.";

            if (bill != null)
            {
                _db.Bills.Remove(bill);
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Message = "Счет успешно удален.";
            }

            return _response;
        }

        public async Task<Response> GetAllBillsAsync()
        {
            var bills = await _db.Bills.ToListAsync();

            _response.IsSuccess = false;
            _response.Message = "Счета не найдены.";

            if (bills.Any())
            {
                _response.IsSuccess = true;
                _response.Result = bills;
                _response.Message = "Счета успешно получены.";
            }

            return _response;
        }

        public async Task<Response> GetBillsInRangeAsync(BillDto billDto)
        {
            var query = _db.Bills.AsQueryable();

            if (billDto.StartDate != null)
                query = query.Where(b => b.CreateDate >= billDto.StartDate.Value);

            if (billDto.EndDate != null)
                query = query.Where(b => b.CreateDate <= billDto.EndDate.Value);

            bool ascending = billDto.Ascending == null ? true : (bool)billDto.Ascending;
            query = ascending
                ? query.OrderBy(b => b.CreateDate)
                : query.OrderByDescending(b => b.CreateDate);

            var bills = await query.ToListAsync();

            _response.IsSuccess = false;
            _response.Message = "Счета не найдены.";

            if (bills.Any())
            {
                _response.IsSuccess = true;
                _response.Result = bills;
                _response.Message = "Счета успешно получены.";
            }

            return _response;
        }

        public async Task<Response> GetBillsByProviderIdAsync(int providerId)
        {
            _response.IsSuccess = false;
            _response.Message = "Счета не найдены для указанного поставщика.";

            var bills = await _db.Bills
                    .Where(p => p.ProviderId == providerId)
                    .ToListAsync();

            if (bills.Any())
            {
                _response.IsSuccess = true;
                _response.Result = bills;
                _response.Message = $"Счета от поставщика с ID {providerId} успешно получены.";
            }

            return _response;
        }

        public async Task<Response> GetBillByIdAsync(int billId)
        {
            var bill = await _db.Bills.FindAsync(billId);

            _response.IsSuccess = false;
            _response.Message = "Счет не найден.";

            if (bill != null)
            {
                _response.IsSuccess = true;
                _response.Result = bill;
                _response.Message = "Счет найден.";

            }

            return _response;
        }
    }
}
