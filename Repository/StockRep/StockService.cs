using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockService.Models;
using StockService.Models.dto;

namespace StockService.Repository.StockRep
{
    public class StockService : IStockService
    {
        private readonly StockContext _db;
        private readonly IMapper _mapper;
        private Response _response;
        public StockService(IMapper mapper, StockContext db)
        {
            _db = db;
            _mapper = mapper;
            _response = new Response();
        }

        public async Task<Response> CreateStockAsync(StockDto stockDto)
        {
            var stockIsExists = await _db.Stocks.AnyAsync(s => s.Name == stockDto.Name);

            _response.IsSuccess = false;
            _response.Message = "Склад уже существует.";
            if (!stockIsExists)
            {
                var stock = _mapper.Map<StockDto, Stock>(stockDto);
                _db.Stocks.Add(stock);

                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Result = stock;
                _response.Message = "Склад добавлен.";
            }

            return _response;
        }

        public async Task<Response> DeleteStockAsync(int stockId)
        {
            var stock = await _db.Stocks.FindAsync(stockId);

            _response.IsSuccess = false;
            _response.Message = "Склад не найден.";

            if (stock != null)
            {
                _db.Stocks.Remove(stock);
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Message = "Склад успешно удален.";
            }

            return _response;
        }

        public async Task<Response> GetAllStocksAsync()
        {
            var stocks = await _db.Stocks.ToListAsync();

            _response.IsSuccess = false;
            _response.Message = "Склады не найдены.";

            if (stocks.Any())
            {
                _response.IsSuccess = true;
                _response.Result = stocks;
                _response.Message = "Склады успешно получены.";
            }

            return _response;
        }

        public async Task<Response> GetStocksByCompanyIdAsync(int companyId)
        {
            _response.IsSuccess = false;
            _response.Message = "Склады не найдены для указанной компании.";

            var stocks = await _db.Stocks
                    .Where(s => s.CompanyId == companyId)
                    .ToListAsync();

            if (stocks.Any())
            {
                _response.IsSuccess = true;
                _response.Result = stocks;
                _response.Message = $"Склады для компании с ID {companyId} успешно получены.";
            }

            return _response;
        }

        public async Task<Response> GetStockByIdAsync(int stockId)
        {
            var stock = await _db.Stocks.FindAsync(stockId);

            _response.IsSuccess = false;
            _response.Message = "Склад не найден.";

            if (stock != null)
            {
                _response.IsSuccess = true;
                _response.Result = stock;
                _response.Message = "Склад найден.";

            }

            return _response;
        }

        public async Task<Response> ChangeStockCompanyAsync(StockDto stockDto)
        {
            var stock = await _db.Stocks.FindAsync(stockDto.StockId);

            _response.IsSuccess = false;
            _response.Message = "Не удалось установить новую компанию для склада.";

            if (stock != null)
            {
                stock.CompanyId = stockDto.CompanyId;

                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Result = stock;

                _response.Message = "Компания установлена в null.";

                if (stock != null)
                {
                    _response.Message = "Компания успешно изменена.";
                }

            }

            return _response;
        }
    }
}
