using System.Text.Json.Serialization;

namespace StockService.Models.dto
{
    public class EmployeeDto
    {
        public string? FullName { get; set; }
        public string? JobTitle { get; set; }

        public string? Login { get; set; }
        public string? Password { get; set; }

        public string? Photo { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public int? StockId { get; set; }
    }
}
