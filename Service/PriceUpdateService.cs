
using HtmlAgilityPack;
using PriceMonitoringService.Data;
using System.Text.RegularExpressions;


namespace PriceMonitoringService.Service
{
    public class PriceUpdateService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly HttpClient _httpClient;


        public PriceUpdateService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _httpClient = new HttpClient();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<PriceMonitorContext>();
                    var subscriptions = context.Subscriptions.ToList();

                    foreach (var subscription in subscriptions)
                    {
                        try
                        {
                           decimal? currentPrice = await GetPriceFromUrlAsync(subscription.Url);

                            if (currentPrice.HasValue && currentPrice != subscription.LastPrice)
                            {
                                EmailService emailService = new EmailService();
                                subscription.LastPrice = currentPrice;
                                subscription.LastChecked = DateTime.UtcNow;

                                //await emailService.SendEmailAsync(subscription.Email, "client", $"уважаемый клиент, спешим уведомить вас, что цена на квартиру {subscription.Url} изменилась с {subscription.LastPrice} на {currentPrice}");
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    await context.SaveChangesAsync();
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task<decimal?> GetPriceFromUrlAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var html = await response.Content.ReadAsStringAsync();
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            var priceNode = doc.DocumentNode.SelectSingleNode("//div[@class='card-flat__price-current']");
            if (priceNode == null)
                return null;
            if (priceNode == null)
            {
                return null;
            }
            var text = priceNode.InnerText.Substring(2).Replace("₽", "").Trim();
            if (decimal.TryParse(text, out var price))
                return price;

            return null;
        }


        
    }
}
