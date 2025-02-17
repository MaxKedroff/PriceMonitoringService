namespace PriceMonitoringService.Data.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Email { get; set; }
        public decimal? LastPrice { get; set; }
        public DateTime LastChecked { get; set; }



    }
}
