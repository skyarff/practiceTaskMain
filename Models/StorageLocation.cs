using System.Text.Json.Serialization;

namespace StockService.Models
{
    public class StorageLocation
    {
        public int StorageLocationId { get; set; }
        public string RackCode { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }

        public int StockId { get; set; }
        public int CompanyId { get; set; }
        [JsonIgnore]
        public Stock? Stock { get; set; }
        [JsonIgnore]
        public List<Product>? Products { get; set; }
        public StorageLocation()
        {
            this.Products = new List<Product>();
        }

        

    }
}
