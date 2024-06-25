namespace StockService.Models
{
    public class Storage
    {
        public int Id { get; set; }
        public int RackNumber { get; set; }
        public int ShelfNubmer { get; set; }
        public string? Description { get; set; }
        public string? StoragePhoto { get; set; }
    }
}
