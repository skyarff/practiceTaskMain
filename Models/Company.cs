namespace StockService.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string? Name { get; set; }
        public string? INN {  get; set; }
        public string? Logo { get; set; }
        public List<Employee> Employees { get; set; }
        public Company()
        {
            this.Employees = new List<Employee>();  
        }
    }
}
