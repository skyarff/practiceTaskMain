using System.Text.Json.Serialization;

namespace StockService.Models
{
    public class UPD
    {
        public int Id { get; set; }
        public string? DocNumber { get; set; }
        public string? PdfScan { get; set; }
        public int ProviderId { get; set; }
        [JsonIgnore]
        public Provider? Provider { get; set; }
    }
}
