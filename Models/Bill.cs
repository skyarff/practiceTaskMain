using System.Text.Json.Serialization;

namespace StockService.Models
{
    public class Bill
    {
        public int BillId { get; set; }
        public string BillNumber { get; set; }
        public string BillPdfPath { get; set; }
        public int ProviderId { get; set; }
        [JsonIgnore]
        public Provider? Provider { get; set; }
        public int CompanyId { get; set; }
        [JsonIgnore]
        public Company? Company { get; set; }
        [JsonIgnore]
        public List<Upd>? Upds { get; set; }


        public Bill()
        {
            this.Upds = new List<Upd>();
        }

        public decimal BillTotal { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
