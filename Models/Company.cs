using System.Text.Json.Serialization;

namespace StockService.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string? INN { get; set; }
        public string? LogoPath { get; set; }

        [JsonIgnore]
        public List<Stock>? Stocks { get; set; }
        [JsonIgnore]
        public List<ProductCategory>? ProductCategories { get; set; }
        public Company()
        {
            this.Stocks = new List<Stock>();
            this.ProductCategories = new List<ProductCategory>();
        }
    }
}
