using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using PriceWatcher.Domain;
using PriceWatcher.Domain.Models;

namespace PriceWatcher.Data.API.Onliner
{
    /// <summary>
    /// Client responsible for fetching product data from Onliner API.
    /// </summary>
    public class OnlinerClient : ICatalogClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OnlinerClient> _logger;
        private readonly string _onlinerSearchUrl = "https://catalog.onliner.by/sdapi/catalog.api/search/products";

        public OnlinerClient(HttpClient httpClient, ILogger<OnlinerClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Fetches a single product by its Onliner key.
        /// </summary>
        /// <param name="key">Unique Onliner product key.</param>
        /// <returns>Mapped Product object or null if not found.</returns>
        public async Task<Product?> FetchProductByKeyAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return null;

            try
            {
                string url = $"{_onlinerSearchUrl}?query={key}";
                var response = await _httpClient.GetFromJsonAsync<OnlinerResponse>(url);

                var onlinerProduct = response?.Products?.FirstOrDefault(p => p.Key == key);
                if (onlinerProduct == null)
                    return null;

                return MapToProduct(onlinerProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product by key: {Key}", key);
                return null;
            }
        }

        /// <summary>
        /// Fetches multiple products by search query.
        /// </summary>
        /// <param name="query">Search query.</param>
        /// <returns>List of mapped Product objects.</returns>
        public async Task<List<Product>> FetchProductsAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<Product>();

            try
            {
                string url = $"{_onlinerSearchUrl}?query={query}";
                var response = await _httpClient.GetFromJsonAsync<OnlinerResponse>(url);

                return response?.Products?.Select(MapToProduct).ToList() ?? new List<Product>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products for query: {Query}", query);
                return new List<Product>();
            }
        }

        /// <summary>
        /// Maps OnlinerProduct to internal Product model.
        /// </summary>
        private static Product MapToProduct(OnlinerProduct p)
        {
            return new Product
            {
                OnlinerKey = p.Key,
                FullName = p.FullName ?? string.Empty,
                PriceMin = TryParseDecimal(p.Prices?.PriceMin?.Amount),
                PriceMax = TryParseDecimal(p.Prices?.PriceMax?.Amount),
                IsTracked = false
            };
        }

        /// <summary>
        /// Safely parses a string to decimal. Returns 0 if parsing fails.
        /// </summary>
        private static decimal TryParseDecimal(string? value)
        {
            return decimal.TryParse(value, out var result) ? result : 0;
        }
    }
}
