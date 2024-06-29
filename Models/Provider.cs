using System.Text.Json.Serialization;

namespace StockService.Models
{
    public class Provider
    {
        public int ProviderId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string? INN { get; set; }
        public string? LegalAdress { get; set; }
        public string? CheckingAccount { get; set; }
        public string? Bank { get; set; }
        public string? BIK { get; set; }
        public string? CorrespondentAccount { get; set; }
        public string? ManagerFullname { get; set; }
        public string? Email { get; set; }
 
        [JsonIgnore]
        public List<Bill>? Bills { get; set; }
        [JsonIgnore]
        public List<Upd>? Upds { get; set; }
        public Provider()
        {
            this.Bills = new List<Bill>();
            this.Upds = new List<Upd>();
        }
    }
}
