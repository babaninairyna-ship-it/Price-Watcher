using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ApplicationDbContext db, ILogger<ProductsController> logger)
    {
        _db = db;
        _logger = logger;
    }

    // POST: api/products/import
    [HttpPost("import")]
    public async Task<IActionResult> ImportProducts([FromBody] List<ProductDto> products)
    {
        if (products == null || products.Count == 0)
        {
            _logger.LogWarning("Empty product list received for import.");
            return BadRequest("Product list is empty.");
        }

        foreach (var dto in products)
        {
            try
            {
                _logger.LogInformation("Processing: {onlinerId} {name}", dto.OnlinerId, dto.FullName);

                var existing = await _db.Products.FirstOrDefaultAsync(p => p.OnlinerId == dto.OnlinerId);
                if (existing != null)
                {
                    existing.FullName = dto.FullName;
                    existing.PriceMin = dto.PriceMin;
                    existing.PriceMax = dto.PriceMax;
                }
                else
                {
                    _db.Products.Add(new Product
                    {
                        OnlinerId = dto.OnlinerId,
                        FullName = dto.FullName,
                        PriceMin = dto.PriceMin,
                        PriceMax = dto.PriceMax
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing product {onlinerId}", dto.OnlinerId);
            }
        }

        try
        {
            await _db.SaveChangesAsync();
            _logger.LogInformation("Saved {count} products to the database.", products.Count);
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Error saving products: {msg}.", dbEx.InnerException?.Message ?? dbEx.Message);
            return StatusCode(500, $"Database error: {dbEx.InnerException?.Message ?? dbEx.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while saving products.");
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }

        return Ok(new { Count = products.Count });
    }

    // GET: api/products
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _db.Products
            .OrderByDescending(p => p.Id)
            .ToListAsync();

        return Ok(products);
    }
}
