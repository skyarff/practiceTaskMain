using System.Text.Json.Serialization;

namespace StockService.Models.dto
{
    public class StockDto
    {
        public string? Name { get; set; }
        public int? CompanyId { get; set; }
    }
}
