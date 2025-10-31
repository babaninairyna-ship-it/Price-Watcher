using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class SearchQueriesController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    public SearchQueriesController(ApplicationDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _db.SearchQueries.ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] SearchQuery query)
    {
        _db.SearchQueries.Add(query);
        await _db.SaveChangesAsync();
        return Ok(query);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var query = await _db.SearchQueries.FindAsync(id);
        if (query == null) return NotFound();
        _db.SearchQueries.Remove(query);
        await _db.SaveChangesAsync();
        return Ok();
    }
}
