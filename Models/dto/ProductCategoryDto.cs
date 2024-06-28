using System.Text.Json.Serialization;

namespace StockService.Models.dto
{
    public class ProductCategoryDto
    {
        public string? Name { get; set; }
        public int? CompanyId { get; set; }
    }
}
