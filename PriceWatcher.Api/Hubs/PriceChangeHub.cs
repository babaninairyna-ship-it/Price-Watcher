using Microsoft.AspNetCore.SignalR;

namespace PriceWatcher.Api.Hubs
{
    public class PriceChangeHub : Hub
    {
        public async Task SendPriceChange(string onlinerKey, decimal oldPriceMin, decimal newPriceMin, decimal oldPriceMax, decimal newPriceMax)
        {
            await Clients.Caller.SendAsync("ReceivePriceChange", onlinerKey, oldPriceMin, newPriceMin, oldPriceMax, newPriceMax);
        }
    }
}
