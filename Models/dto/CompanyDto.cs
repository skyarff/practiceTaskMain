using System.Text.Json.Serialization;

namespace StockService.Models.dto
{
    public class CompanyDto
    {
        public int? CompanyId { get; set; }
        public string? Name { get; set; }
        public string? INN { get; set; }
        public string? Logo { get; set; }
    }
}
