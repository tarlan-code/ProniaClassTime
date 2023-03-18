using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Pronia.Data;
using Pronia.Models;
using System.Net.WebSockets;

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
            Client client = new Client
            {
                ConnectionId = Context.ConnectionId,
                Username = _accessor.HttpContext.User.Identity.Name
            };

            ClientSource.Clients.Add(client);
            await Clients.All.SendAsync("SetOnline", ClientSource.Clients);
            await base.OnConnectedAsync();
        }


        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var client = ClientSource.Clients.FirstOrDefault(c=>c.ConnectionId == Context.ConnectionId);
            if (client is null) throw new Exception();
            else ClientSource.Clients.Remove(client);
            return base.OnDisconnectedAsync(exception);
        }


    }
}
