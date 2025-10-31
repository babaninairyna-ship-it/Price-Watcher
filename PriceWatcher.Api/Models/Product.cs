using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

public class Product
{
    [Key]
    public int Id { get; set; }

    public long OnlinerId { get; set; }

    public string FullName { get; set; } = string.Empty;

    [Precision(18, 2)]
    public decimal PriceMin { get; set; }

    [Precision(18, 2)]
    public decimal PriceMax { get; set; }
}
