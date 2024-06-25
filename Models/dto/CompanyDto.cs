namespace StockService.Models.dto
{
    public class CompanyDto
    {
        public string Name { get; set; }
        public string INN { get; set; }
        public string Logo { get; set; }
        public List<Employee> Employees { get; set; }
        public CompanyDto()
        {
            this.Employees = new List<Employee>();
        }
    }
}
