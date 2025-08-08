using System.Net.Http;

namespace WebApiBudget.Services
{
    public class KeepAliveService : BackgroundService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<KeepAliveService> _logger;
        private readonly IConfiguration _configuration;
        private readonly TimeSpan _interval;

        public KeepAliveService(
            IHttpClientFactory httpClientFactory, 
            ILogger<KeepAliveService> logger,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
            
            // Set ping interval (default: 10 minutes)
            var intervalMinutes = _configuration.GetValue<int>("KeepAlive:IntervalMinutes", 10);
            _interval = TimeSpan.FromMinutes(intervalMinutes);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Keep-Alive service started. Ping interval: {Interval}", _interval);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await PingApi();
                    await Task.Delay(_interval, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    // Service is stopping
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while pinging API");
                    // Continue running even if one ping fails
                    await Task.Delay(_interval, stoppingToken);
                }
            }

            _logger.LogInformation("Keep-Alive service stopped");
        }

        private async Task PingApi()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                
                // Get the base URL from configuration
                var baseUrl = _configuration.GetValue<string>("KeepAlive:BaseUrl") 
                             ?? "https://localhost:5001"; // Fallback to localhost for development

                // Use a simple endpoint that doesn't require authentication
                var pingUrl = $"{baseUrl}/api/ping";

                var response = await httpClient.GetAsync(pingUrl);
                
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Keep-alive ping successful at {Time}", DateTime.UtcNow);
                }
                else
                {
                    _logger.LogWarning("Keep-alive ping returned status: {StatusCode} at {Time}", 
                        response.StatusCode, DateTime.UtcNow);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to ping API for keep-alive at {Time}", DateTime.UtcNow);
            }
        }
    }
}