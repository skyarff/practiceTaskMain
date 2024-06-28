using System.Text.Json.Serialization;

namespace StockService.Models.dto
{
    public class ProviderDto
    {
        public string Name { get; set; }
        public string INN { get; set; }
        public string LegalAdress { get; set; }
        public string CheckingAccount { get; set; }
        public string Bank { get; set; }
        public string BIK { get; set; }
        public string CorrespondentAccount { get; set; }
        public string ManagerFullname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
