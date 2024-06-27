using System.Text.Json.Serialization;

namespace StockService.Models
{
    public class Stock
    {
        public int StockId { get; set; }
        public string Name { get; set; }

        public int? CompanyId { get; set; }
        [JsonIgnore]
        public Company? Company { get; set; }
        [JsonIgnore]
        public List<Employee>? Employees { get; set; }
        [JsonIgnore]
        public List<StorageLocation>? StorageLocations { get; set; }
        public Stock()
        {
            this.Employees = new List<Employee>();
            this.StorageLocations = new List<StorageLocation>();
        }
    }
}
