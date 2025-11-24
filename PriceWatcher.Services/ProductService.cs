using PriceWatcher.Domain;
using PriceWatcher.Domain.Models;
using PriceWatcher.Services.Interfaces;

namespace PriceWatcher.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly ICatalogClient _catalogClient;

        public ProductService(IProductRepository productRepo, ICatalogClient catalogClient)
        {
            _productRepo = productRepo;
            _catalogClient = catalogClient;
        }

        /// <summary>
        /// Fetch products from Onliner and merge with tracked state.
        /// </summary>
        public async Task<List<Product>> SearchWithTrackingAsync(string search)
        {
            var fetched = await _catalogClient.FetchProductsAsync(search);
            var tracked = await _productRepo.GetTrackedProductsAsync();

            foreach (var p in fetched)
            {
                var t = tracked.FirstOrDefault(x => x.OnlinerKey == p.OnlinerKey);
                if (t != null)
                    p.IsTracked = t.IsTracked;
            }

            return fetched;
        }

        /// <summary>
        /// Track or untrack product.
        /// </summary>
        public async Task ToggleTrackingAsync(string key, bool state)
        {
            Product? product;

            if (state)
            {
                product = await _catalogClient.FetchProductByKeyAsync(key);
                if (product == null)
                    throw new Exception("Product not found");
            }
            else
            {
                product = await _productRepo.GetByKeyAsync(key);
                if (product == null)
                    throw new Exception("Product not found");
            }

            product.IsTracked = state;
            await _productRepo.SaveProductsAsync(new List<Product> { product });
        }

        /// <summary>
        /// Get tracked products.
        /// </summary>
        public async Task<List<Product>> GetTrackedAsync()
        {
            return await _productRepo.GetTrackedProductsAsync();
        }
    }
}
