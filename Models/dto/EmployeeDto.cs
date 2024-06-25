using System.Text.Json.Serialization;

namespace StockService.Models.dto
{
    public class EmployeeDto
    {
        public string FullName { get; set; }
        public int CompanyId { get; set; }
        public string? JobTitle { get; set; } //
        public string? Photo { get; set; }//
        public string? Email { get; set; }//
        public string? Phone { get; set; } //
    }
}
