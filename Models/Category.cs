namespace StockService.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Company Company { get; set; }
    }
}
