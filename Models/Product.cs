using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Text.Json.Serialization;

namespace StockService.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Manufacturer { get; set; }
        public string? ProductionArticle { get; set; }
        public string? InnerArticle { get; set; }
        public string? Photo { get; set; }
        public decimal Price { get; set; }
        public string? FactoryNumber { get; set; }

        public int BillId { get; set; }
        [JsonIgnore]
        public Bill? Bill { get; set; }
        public int UPDId { get; set; }
        [JsonIgnore]
        public UPD? UPD { get; set; }
        public int ProviderId { get; set; }
        [JsonIgnore]
        public Provider? Provider { get; set; }
        public int CategoryId { get; set; }
        [JsonIgnore]
        public Category? Category { get; set; }
        public int StorageId { get; set; }
        [JsonIgnore]
        public Storage? Storage { get; set; }
        public int StockId { get; set; }
        [JsonIgnore]
        public Stock? Stock { get; set; }
        public int EmployeeId { get; set; }
        [JsonIgnore]
        public Employee? Employee { get; set; }
    }
}
