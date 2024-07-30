using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;

namespace StockService.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }

        public string Login { get; set; }
        [JsonIgnore]
        public string? PasswordHash { get; set; }
        public string Role { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }


        public int? StockId { get; set; }
        [JsonIgnore]
        public Stock? Stock { get; set; }
        [JsonIgnore]
        public List<Product>? Products { get; set; }
        public Employee()
        {
            this.Products = new List<Product>();
        }

        public string? ImagePath { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public int? CompanyId { get; set; }
    }
}
