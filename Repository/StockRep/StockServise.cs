using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockService.Models.dto;
using StockService.Models;

namespace StockService.Repository.StockRep
{
    public class StockServise : IStockService
    {
        private readonly StockContext _db;
        private readonly IMapper _mapper;
        private Response _response;
        public StockServise(IMapper mapper, StockContext db)
        {
            _db = db;
            _mapper = mapper;
            _response = new Response();
        }

        public async Task<Response> CreateStockAsync(StockDto stockDto)
        {
            Response response = new Response();
            var stockIsExists = await _db.Stocks.AnyAsync(s => s.Name == stockDto.Name);

            response.IsSuccess = false;
            response.Message = "Склад уже существует.";
            if (!stockIsExists)
            {
                var stock = _mapper.Map<StockDto, Stock>(stockDto);
                _db.Stocks.Add(stock);

                await _db.SaveChangesAsync();

                response.IsSuccess = true;
                response.Result = stock;
                response.Message = "Склад добавлен.";
            }
            return response;
        }

        public async Task<Response> DeleteStockAsync(int id)
        {
            Response response = new Response();
            var stock = await _db.Stocks.FindAsync(id);

            response.IsSuccess = true;
            response.Message = "Склад не найден.";

            if (stock != null)
            {
                response.IsSuccess = false;
                response.Message = "Склад успешно удален.";

                _db.Stocks.Remove(stock);
                await _db.SaveChangesAsync();
            }

            return response;
        }

        public async Task<Response> GetAllStocksAsync()
        {
            Response response = new Response();

            try
            {
                var stocks = await _db.Stocks.ToListAsync();
                response.IsSuccess = false;
                response.Message = "Склады не найдены.";

                if (stocks.Count != 0)
                {
                    response.IsSuccess = true;
                    response.Result = stocks;
                    response.Message = "Склады успешно получены.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors.Add(ex.Message);
            }

            return response;
        }

        public async Task<Response> GetStockByIdAsync(int id)
        {
            Response response = new Response();
            var stock = await _db.Stocks.FirstOrDefaultAsync(s => s.StockId == id);

            response.IsSuccess = false;
            response.Message = "Склад не найден.";
            if (stock != null)
            {
                response.IsSuccess = true;
                response.Result = stock;
                response.Message = "Склад найден.";

            }

            return response;
        }

        public async Task<Response> UpdateStockAsync(int id, StockDto stockDto)
        {
            Response response = new Response();
            var stock = await _db.Stocks.FirstOrDefaultAsync(s => s.StockId == id);

            response.IsSuccess = false;
            response.Message = "Склад не найден.";

            if (stock != null)
            {
                if (!string.IsNullOrEmpty(stockDto.Name))
                    stock.Name = stockDto.Name;

                if (stockDto.CompanyId != 0)
                    stock.CompanyId = stockDto.CompanyId;


                await _db.SaveChangesAsync();

                response.IsSuccess = true;
                response.Result = stock;
                response.Message = "Данные склада обновлены.";
            }

            return response;
        }
    }
}
