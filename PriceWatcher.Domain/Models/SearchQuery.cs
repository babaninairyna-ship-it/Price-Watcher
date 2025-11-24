namespace PriceWatcher.Domain.Models
{
    public class SearchQuery
    {
        public int Id { get; set; }
        public string Query { get; set; } = null!;
    }
}