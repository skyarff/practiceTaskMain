using System.Text.Json.Serialization;

namespace StockService.Models
{
    public class StorageLocation
    {
        public int StorageLocationId { get; set; }
        public int RackCode { get; set; }
        public int ShelfCode { get; set; }
        public string? Description { get; set; }
        public string? LocationPhoto { get; set; }

        public int? StockId { get; set; }
        [JsonIgnore]
        public Stock? Stock { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; }

    }
}
