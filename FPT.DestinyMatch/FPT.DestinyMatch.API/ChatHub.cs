using Microsoft.AspNetCore.SignalR;
namespace FPT.DestinyMatch.API
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(Guid conversationId, string message)
        {
            await Clients.Group(conversationId.ToString()).SendAsync("ReceiveMessage", conversationId, message);
        }

        public Task JoinConversation(string conversationId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
        }
    }

}
