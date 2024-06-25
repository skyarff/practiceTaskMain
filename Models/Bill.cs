using System.Text.Json.Serialization;

namespace StockService.Models
{
    public class Bill
    {
        public int BillId { get; set; }
        public string? BillNumber { get; set; }
        public string? PdfBill { get; set; }
        public int ProviderId { get; set; }
        [JsonIgnore]
        public Provider? Provider { get; set; }
        public decimal Price { get; set; }
    }
}
