using AppSettings;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockService.Models;
using StockService.Models.dto;

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

                string filePath = "";
                if (billDto.BillPdf != null && billDto.BillPdf.Length > 0)
                {
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(billDto.BillPdf.FileName)}";
                    filePath = Path.Combine(_imagePath, fileName);
                    bill.BillPdfPath = filePath;
                }

                bill.CreateDate = DateTime.UtcNow;
                _db.Bills.Add(bill);
                await _db.SaveChangesAsync();

                if(!string.IsNullOrEmpty(filePath))
                    using (var stream = new FileStream("wwwroot/" + filePath, FileMode.Create))
                        await billDto.BillPdf.CopyToAsync(stream);

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

                if (File.Exists("wwwroot//" + bill.BillPdfPath))
                    File.Delete("wwwroot//" + bill.BillPdfPath);

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

            //bool ascending = billDto.Ascending == null ? true : (bool)billDto.Ascending;
            query = true
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

        public async Task<Response> GetBillsByProviderAndCompanyIdAsync(int? providerId, int? companyId)
        {
            _response.IsSuccess = false;
            _response.Message = "Счета не найдены для указанного поставщика и компании.";

            var query = _db.Bills.AsQueryable();

            if (providerId != null)
                query = query.Where(b => b.ProviderId == providerId);
            if (companyId != null)
                query = query.Where(b => b.CompanyId == companyId);

            var bills = await query.ToListAsync();

            if (bills.Any())
            {
                _response.IsSuccess = true;
                _response.Result = bills;
                _response.Message = $"Счета от поставщика с ID {providerId} и компании с ID {companyId} успешно получены.";
            }

            return _response;
        }

        public async Task<Response> GetBillsByCompanyIdAsync(int companyId)
        {
            _response.IsSuccess = false;
            _response.Message = "Счета не найдены для указанной компании.";

            var bills = await _db.Bills
                    .Where(с => с.CompanyId == companyId)
                    .ToListAsync();

            if (bills.Any())
            {
                _response.IsSuccess = true;
                _response.Result = bills;
                _response.Message = $"Счета для компании с ID {companyId} успешно получены.";
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

        public async Task<Response> GetBillsFilteredAsync(BillDto billDto)
        {
            _response.IsSuccess = false;
            _response.Message = "Счета не найдены по указанным критериям.";

            var query = _db.Bills.AsQueryable();

            if (billDto.BillId != null)
                query = query.Where(b => b.BillId == billDto.BillId);

            if (!string.IsNullOrEmpty(billDto.BillNumber))
                query = query.Where(b => EF.Functions.ILike(b.BillNumber, $"%{billDto.BillNumber}%"));

            if (billDto.ProviderId != null)
                query = query.Where(b => b.ProviderId == billDto.ProviderId);

            if (billDto.CompanyId != null)
                query = query.Where(b => b.CompanyId == billDto.CompanyId);


            if (billDto.LowerBillTotalLimit != null)
                query = query.Where(b => b.BillTotal >= billDto.LowerBillTotalLimit);
            if (billDto.UpperBillTotalLimit != null)
                query = query.Where(b => b.BillTotal <= billDto.UpperBillTotalLimit);


            if (billDto.StartDate != null)
                query = query.Where(b => b.CreateDate >= billDto.StartDate.Value);
            if (billDto.EndDate != null)
                query = query.Where(b => b.CreateDate <= billDto.EndDate.Value);

            var bills = await query.ToListAsync();
            if (bills.Any())
            {
                _response.IsSuccess = true;
                _response.Result = bills;
                _response.Message = "Счета успешно найдены по указанным критериям.";
            }

            return _response;
        }
    }
}
