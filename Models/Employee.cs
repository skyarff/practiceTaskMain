﻿using System.Text.Json.Serialization;

namespace StockService.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public int CompanyId { get; set; }
        public string? FullName { get; set; }
        [JsonIgnore]
        public Company? Company { get; set; }
        public string? JobTitle { get; set; }
        public string? Photo { get; set; }//
        public string? Email { get; set; }//
        public string? Phone { get; set; } //
    }
}