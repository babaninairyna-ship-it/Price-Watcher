using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PriceWatcher.Api.Hubs;
using PriceWatcher.Services.Messages;

namespace PriceWatcher.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestHubController : ControllerBase
    {
        private readonly IHubContext<PriceChangeHub> _hubContext;

        public TestHubController(IHubContext<PriceChangeHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("send-test")]
        public async Task<IActionResult> SendTestMessage()
        {
            var testMessage = new PriceChangedMessage
            {
                OnlinerKey = "test-product-1",
                OldPriceMin = 100,
                NewPriceMin = 120,
                OldPriceMax = 150,
                NewPriceMax = 180,
                ChangedAt = DateTime.UtcNow
            };

            await _hubContext.Clients.All.SendAsync("ReceivePriceChange", testMessage);

            return Ok(new { status = "Message sent!", message = testMessage });
        }
    }
}
