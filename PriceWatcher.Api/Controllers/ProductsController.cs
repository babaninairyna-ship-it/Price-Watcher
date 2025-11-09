using Microsoft.AspNetCore.Mvc;
using PriceWatcher.Data.Repositories;
using CatalogLoader.Services;
using PriceWatcher.Data.Models;

namespace PriceWatcher.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductRepository _productRepo;
        private readonly OnlinerClient _onlinerClient;

        public ProductsController(
            ProductRepository productRepo,
            OnlinerClient onlinerClient)
        {
            _productRepo = productRepo;
            _onlinerClient = onlinerClient;
        }

        /// <summary>
        /// GET api/products?search=iphone
        /// Fetch products from Onliner API and merge with tracked status.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return BadRequest("Search query cannot be empty.");

            var fetchedProducts = await _onlinerClient.FetchProductsAsync(search);
            var trackedProducts = await _productRepo.GetTrackedProductsAsync();

            var merged = fetchedProducts.Select(p =>
            {
                var tracked = trackedProducts.FirstOrDefault(t => t.OnlinerKey == p.OnlinerKey);
                if (tracked != null)
                    p.IsTracked = tracked.IsTracked;

                return p;
            }).ToList();

            return Ok(merged);
        }

        /// <summary>
        /// POST api/products/{key}/track
        /// Mark product as tracked.
        /// </summary>
        [HttpPost("{key}/track")]
        public async Task<IActionResult> Track(string key)
        {
            var product = await _onlinerClient.FetchProductByKeyAsync(key);
            if (product == null)
                return NotFound("Product not found.");

            product.IsTracked = true;
            await _productRepo.SaveProductsAsync(new List<Product> { product });

            return Ok();
        }

        /// <summary>
        /// POST api/products/{key}/untrack
        /// Unmark product as tracked.
        /// </summary>
        [HttpPost("{key}/untrack")]
        public async Task<IActionResult> Untrack(string key)
        {
            var product = await _productRepo.GetByKeyAsync(key);
            if (product == null)
                return NotFound("Product not found.");

            product.IsTracked = false;
            await _productRepo.SaveProductsAsync(new List<Product> { product });

            return Ok();
        }

        /// <summary>
        /// GET api/products/tracked
        /// Get all tracked products.
        /// </summary>
        [HttpGet("tracked")]
        public async Task<IActionResult> GetTracked()
        {
            var trackedProducts = await _productRepo.GetTrackedProductsAsync();
            return Ok(trackedProducts);
        }
    }
}
