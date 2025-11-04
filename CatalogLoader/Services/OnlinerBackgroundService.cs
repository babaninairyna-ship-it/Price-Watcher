using System.Net.Http.Json;

public class OnlinerBackgroundService : BackgroundService
{
    private readonly ILogger<OnlinerBackgroundService> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl;

    public OnlinerBackgroundService(
        ILogger<OnlinerBackgroundService> logger,
        IHttpClientFactory httpClientFactory,
        IConfiguration config)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient();
        _apiBaseUrl = config["ApiSettings:BaseUrl"]!;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var queries = new List<string> { "xiaomi", "samsung", "iphone" };

        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var query in queries)
            {
                await FetchAndSendProducts(query);
            }

            _logger.LogInformation("Waiting 10 minutes until the next cycle...");
            await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
        }
    }

    private async Task FetchAndSendProducts(string query)
    {
        try
        {
            _logger.LogInformation("Fetching products from Onliner: {query}.", query);

            string onlinerUrl = $"https://catalog.onliner.by/sdapi/catalog.api/search/products?query={query}";
            var onlinerResponse = await _httpClient.GetFromJsonAsync<OnlinerResponse>(onlinerUrl);

            if (onlinerResponse?.Products == null || !onlinerResponse.Products.Any())
            {
                _logger.LogInformation("No products found for query '{query}'.", query);
                return;
            }

            var productDtos = onlinerResponse.Products.Select(p => new ProductDto
            {
                OnlinerId = p.Id,
                FullName = p.FullName,
                PriceMin = decimal.Parse(p.Prices.PriceMin.Amount),
                PriceMax = decimal.Parse(p.Prices.PriceMax.Amount)
            }).ToList();

            _logger.LogInformation("Found {count} products for query '{query}'.", productDtos.Count, query);

            string apiUrl = $"{_apiBaseUrl}/api/products/import";
            var apiResponse = await _httpClient.PostAsJsonAsync(apiUrl, productDtos);

            if (apiResponse.IsSuccessStatusCode)
            {
                _logger.LogInformation("Products successfully sent to API.");
            }
            else
            {
                var error = await apiResponse.Content.ReadAsStringAsync();
                _logger.LogError("Error sending products to the API: {error}.", error);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching data from Onliner for query '{query}'.", query);
        }
    }
}