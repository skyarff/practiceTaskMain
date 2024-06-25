using Microsoft.EntityFrameworkCore.Infrastructure;

namespace StockService.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string ProductionArticle { get; set; }
        public string InnerArticle { get; set; }
        public string Photo { get; set; }
        public decimal Price { get; set; }
        public string FactoryNumber { get; set; }
        public Bill Bill { get; set; }
        public UPD UPD { get; set; }
        public Provider Provider { get; set; }
        public Category Category { get; set; }
        public Storage Storage { get; set; }
        public Stock Stock { get; set; }
        public Employee Employee { get; set; }
    }
}
