using System.Text.Json.Serialization;

namespace StockService.Models.dto
{
    public class BillDto
    {
        public int? BillId { get; set; }
        public string? BillNumber { get; set; }
        public IFormFile? BillPdf { get; set; }
        public int? ProviderId { get; set; }

        public decimal? BillTotal { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Ascending { get; set; }
    }
}
