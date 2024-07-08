using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace FPT.DestinyMatch.API
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(Guid conversationId, string message)
        {
            await Clients.Client(conversationId.ToString()).SendAsync("ReceiveMessage", conversationId, message);
        }      
    }
}
