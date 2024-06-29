using System.Text.Json.Serialization;

namespace StockService.Models.dto
{
    public class StockDto
    {
        public int? StockId { get; set; }
        public string? Name { get; set; }
        public int? CompanyId { get; set; }
    }
}
