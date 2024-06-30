using System.Text.Json.Serialization;

namespace StockService.Models.dto
{
    public class StorageLocationDto
    {
        public int? StorageLocationId { get; set; }
        public string? RackCode { get; set; }
        public string? ShelfCode { get; set; }
        public string? Description { get; set; }

        public int? StockId { get; set; }
        public IFormFile? Image { get; set; }
        public bool? IsBusy { get; set; }

    }
}
