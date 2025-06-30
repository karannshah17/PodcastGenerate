using Microsoft.Extensions.Options;
using PodcastGenerate.Interface;
using PodcastGenerate.Models.Configurations;
using System.Text;

namespace PodcastGenerate.Services
{
    public class FirecrawlScraper : IContentScraper
    {
        private readonly HttpClient _httpClient;
        private readonly FirecrawlConfig _config;
        private readonly ILogger<FirecrawlScraper> _logger;

        public FirecrawlScraper(IOptions<FirecrawlConfig> config, ILogger<FirecrawlScraper> logger)
        {
            _config = config.Value ?? throw new ArgumentNullException(nameof(config));
            _httpClient = new HttpClient();

            // Validate configuration
            if (string.IsNullOrWhiteSpace(_config.ApiKey))
                throw new ArgumentException("Firecrawl API key is required");

            if (string.IsNullOrWhiteSpace(_config.Endpoint))
                throw new ArgumentException("Firecrawl endpoint is required");
            _logger = logger;
        }

        public async Task<string> ScrapeContentAsync(string url)
        {
            try
            {
                var jsonBody = $@"{{
          ""formats"": [""markdown""],
          ""onlyMainContent"": true,
          ""maxAge"": 0,
          ""waitFor"": 0,
          ""mobile"": false,
          ""skipTlsVerification"": false,
          ""timeout"": 30000,
          ""parsePDF"": true,
          ""location"": {{ ""country"": ""US"" }},
          ""blockAds"": true,
          ""storeInCache"": true,
          ""url"": ""{url}""
        }}";

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_config.ApiKey}");

                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_config.Endpoint, content);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed while fetching content for URL: {Url}", url);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching content for URL: {Url}", url);
                return null;
            }
        }
    }
}
