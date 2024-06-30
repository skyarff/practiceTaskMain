using System.Text.Json.Serialization;

namespace StockService.Models
{
    public class Bill
    {
        public int BillId { get; set; }
        public string BillNumber { get; set; }
        public string? BillPdfPath { get; set; }
        public int ProviderId { get; set; }
        [JsonIgnore]
        public Provider? Provider { get; set; }
        [JsonIgnore]
        public List<Product>? Products { get; set; }

        public Bill()
        {
            this.Products = new List<Product>();
        }

        public decimal BillTotal { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
