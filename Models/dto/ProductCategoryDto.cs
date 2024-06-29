using System.Text.Json.Serialization;

namespace StockService.Models.dto
{
    public class ProductCategoryDto
    {
        public int? ProductCategoryId { get; set; }
        public string? Name { get; set; }
        public int? CompanyId { get; set; }
    }
}
