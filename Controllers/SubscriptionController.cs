using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceMonitoringService.Data;
using PriceMonitoringService.Data.DTO;
using PriceMonitoringService.Data.Models;

namespace PriceMonitoringService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly PriceMonitorContext _context;

        public SubscriptionController(PriceMonitorContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> subscribe([FromBody] SubscriptionDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Url) || string.IsNullOrWhiteSpace(dto.Email))
            {
                return BadRequest("необходимо передать Url и Email");
            }

            var subscription = new Subscription
            {
                Url = dto.Url,
                Email = dto.Email,
                LastChecked = DateTime.UtcNow
            };

            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            return Ok(subscription);
        }

        [HttpGet("prices")]
        public async Task<IActionResult> GetPrices()
        {
            var subscriptions = await _context.Subscriptions.ToListAsync();
            var result = subscriptions.Select(subscriptions => new
            {
                subscriptions.Url,
                subscriptions.LastPrice,
                subscriptions.LastChecked
            });

            return Ok(result);
        }
    }
}
