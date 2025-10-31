public class ProductDto
{
    public long OnlinerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public decimal PriceMin { get; set; }
    public decimal PriceMax { get; set; }
}