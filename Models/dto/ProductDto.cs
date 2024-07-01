using Microsoft.Extensions.Logging;
using StockService.Models;
using System.Text.Json.Serialization;

namespace StockService.Models.dto
{
    public class ProductDto
    {
        public int? ProductId { get; set; }
        public string? Name { get; set; }
        public string? Manufacturer { get; set; }
        public string? ProductionArticle { get; set; }
        public string? InnerArticle { get; set; }
        public IFormFile? Image { get; set; }
        public string? FactoryNumber { get; set; }

        public decimal? LowerPriceLimit { get; set; }
        public decimal? Price { get; set; }
        public decimal? UpperPriceLimit { get; set; }

        public int? BillId { get; set; }
        public int? UpdId { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? StorageLocationId { get; set; }
        public int? EmployeeId { get; set; }


        public int? StockId { get; set; }
        public int? CompanyId { get; set; }
        public int? ProviderId { get; set; }
        

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Ascending { get; set; }
    }
}
