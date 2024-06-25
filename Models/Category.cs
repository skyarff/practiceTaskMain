using System.Text.Json.Serialization;

namespace StockService.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int CompanyId { get; set; }
        [JsonIgnore]
        public Company? Company { get; set; }
    }
}
