using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace FPT.DestinyMatch.API
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(Guid conversationId, string message)
        {
            await Clients.Group(conversationId.ToString()).SendAsync("ReceiveMessage", conversationId, message);
        }

        public async Task JoinConversation(string conversationId)
        {
            if (string.IsNullOrWhiteSpace(conversationId))
            {
                throw new ArgumentException("Conversation ID cannot be null or empty", nameof(conversationId));
            }

            var connectionId = Context.ConnectionId;
            if (string.IsNullOrWhiteSpace(connectionId))
            {
                throw new InvalidOperationException("Connection ID cannot be null or empty");
            }

            try
            {
                await Groups.AddToGroupAsync(connectionId, conversationId);
                Console.WriteLine($"Connection {connectionId} added to group {conversationId}");
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error joining group {conversationId}: {ex.Message}");
                throw;
            }
        }

        
    }
}
