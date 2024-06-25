namespace StockService.Models
{
    public class UPD
    {
        public int Id { get; set; }
        public string DocNumber { get; set; }
        public string PdfScan { get; set; }
        public Provider Provider { get; set; }
    }
}
