namespace StockService.Models
{
    public class Bill
    {
        public int Id { get; set; }
        public string BillNumber { get; set; }
        public string PdfBill { get; set; }
        public Provider Provider { get; set; }
        public decimal Price { get; set; }
    }
}
