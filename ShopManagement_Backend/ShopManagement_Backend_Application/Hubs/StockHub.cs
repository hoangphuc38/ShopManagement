using Microsoft.AspNetCore.SignalR;

namespace ShopManagement_Backend_Application.Hubs
{
    public class StockHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.ConnectionId} has joined to ChatHub");
            await base.OnConnectedAsync();
        }
    }
}
