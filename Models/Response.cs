namespace StockService.Models
{
    public class Response
    {
        public bool IsSuccess { get; set; }
        public object Result { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new List<string>();
        public string Message { get; set; } = string.Empty;
    }
}
