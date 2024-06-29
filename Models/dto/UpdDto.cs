using System.Text.Json.Serialization;

namespace StockService.Models.dto
{
    public class UpdDto
    {
        public int? UpdId { get; set; }
        public string? DocumentNumber { get; set; }
        public IFormFile? UpdPdf { get; set; }
        public int? ProviderId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Ascending { get; set; }
    }
}
