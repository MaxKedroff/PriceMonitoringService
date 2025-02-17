using Microsoft.EntityFrameworkCore;
using PriceMonitoringService.Data.Models;

namespace PriceMonitoringService.Data
{
    public class PriceMonitorContext : DbContext
    {
        public PriceMonitorContext(DbContextOptions<PriceMonitorContext> options)
            : base(options) { }

        public DbSet<Subscription> Subscriptions { get; set; }
    }
}
