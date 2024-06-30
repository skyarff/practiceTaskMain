using System.Text.Json.Serialization;

namespace StockService.Models
{
    public class Upd
    {
        public int UpdId { get; set; }
        public string DocumentNumber { get; set; }
        public string? UpdPdfPath { get; set; }
        public int ProviderId { get; set; }
        [JsonIgnore]
        public Provider? Provider { get; set; }
        [JsonIgnore]
        public List<Product>? Products { get; set; }
        public DateTime CreateDate { get; set; }
        public Upd()
        {
            this.Products = new List<Product>();
        }
    }
}
