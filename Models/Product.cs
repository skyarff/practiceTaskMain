using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Text.Json.Serialization;

namespace StockService.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string? Manufacturer { get; set; }
        public string? ProductionArticle { get; set; }
        public string? InnerArticle { get; set; }
        public string? Photo { get; set; }
        public decimal Price { get; set; }
        public string? FactoryNumber { get; set; }
        public DateTime CreateDate { get; set; }

        public int? BillId { get; set; }
        [JsonIgnore]
        public Bill? Bill { get; set; }
        public int? UpdId { get; set; }
        [JsonIgnore]
        public Upd? Upd { get; set; }
        public int? ProductCategoryId { get; set; }
        [JsonIgnore]
        public ProductCategory? ProductCategory { get; set; }
        public int StorageLocationId { get; set; }
        [JsonIgnore]
        public StorageLocation StorageLocation { get; set; }
        public int EmployeeId { get; set; }
        [JsonIgnore]
        public Employee Employee { get; set; }
    }
}
