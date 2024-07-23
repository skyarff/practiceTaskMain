using System.Text.Json.Serialization;

namespace StockService.Models.output
{
    public class ProductOut
    {
        public int? ProductId { get; set; }
        public string? Name { get; set; }
        public string? Manufacturer { get; set; }
        public string? ProductionArticle { get; set; }
        public string? InnerArticle { get; set; }
        public string? FactoryNumber { get; set; }
        public decimal? Price { get; set; }
        public string? ImagePath { get; set; }
        public DateTime? CreateDate { get; set; }


        public int? UpdId { get; set; }
        public string? DocumentNumber { get; set; }

        public int? ProductCategoryId { get; set; }
        public string? ProductCategoryName { get; set; }

        public int? StorageLocationId { get; set; }
        public string? RackCode { get; set; }
        public string? ShelfCode { get; set; }

        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }

        public int StockId { get; set; }
        public string? StockName { get; set; }

        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }

        public int? BillId { get; set; }
        public string? BillNumber { get; set; }

        public int? ProviderId { get; set; }
        public string? ProviderName { get; set; }
    }
}
