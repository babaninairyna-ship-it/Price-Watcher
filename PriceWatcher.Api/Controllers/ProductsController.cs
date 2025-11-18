using Microsoft.AspNetCore.Mvc;
using PriceWatcher.Services.Interfaces;

namespace PriceWatcher.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// GET api/products?search=iphone
        /// Search products on Onliner + merge tracking state.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return BadRequest("Search query cannot be empty.");

            var result = await _productService.SearchWithTrackingAsync(search);
            return Ok(result);
        }

        /// <summary>
        /// POST api/products/{key}/track
        /// Set tracked = true.
        /// </summary>
        [HttpPost("{key}/track")]
        public async Task<IActionResult> Track(string key)
        {
            await _productService.ToggleTrackingAsync(key, true);
            return Ok();
        }

        /// <summary>
        /// POST api/products/{key}/untrack
        /// Set tracked = false.
        /// </summary>
        [HttpPost("{key}/untrack")]
        public async Task<IActionResult> Untrack(string key)
        {
            await _productService.ToggleTrackingAsync(key, false);
            return Ok();
        }

        /// <summary>
        /// GET api/products/tracked
        /// Returns all tracked products.
        /// </summary>
        [HttpGet("tracked")]
        public async Task<IActionResult> GetTracked()
        {
            var result = await _productService.GetTrackedAsync();
            return Ok(result);
        }
    }
}
