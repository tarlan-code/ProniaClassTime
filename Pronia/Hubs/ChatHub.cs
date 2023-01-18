using Microsoft.AspNetCore.SignalR;

namespace Pronia.Hubs
{
    public class ChatHub:Hub
    {
        IHttpContextAccessor _accessor { get; }

        public ChatHub(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public async Task SendMessage(string message)
        {
            string username = String.Empty;
            if (_accessor.HttpContext.User.Identity.IsAuthenticated) username = _accessor.HttpContext.User.Identity.Name;
            else throw new Exception();
            await Clients.All.SendAsync("ReceiveMessage", message,username);
        }

        public async override Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("SetOnline", _accessor.HttpContext.User.Identity.Name);
            await base.OnConnectedAsync();
        }


    }
}
