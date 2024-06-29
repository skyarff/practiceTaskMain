using System.Text.Json.Serialization;

namespace StockService.Models
{
    public class ProductCategory
    {
        public int ProductCategoryId { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }

        [JsonIgnore]
        public Company? Company { get; set; }
        [JsonIgnore]
        public List<Product>? Products { get; set; }
        public ProductCategory()
        {
            this.Products = new List<Product>();
        }
    }
}
