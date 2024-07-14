using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace FPT.DestinyMatch.API
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string matchId, string memberId, string message)
        {
            Console.WriteLine($"Message sent from {memberId} to ${matchId} with content ${message}");
            try
            {
                await Clients.Group(matchId).SendAsync("ReceiveMessage", memberId, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
                throw;
            }
        }

        public async Task JoinGroup(string memberId, string matchId)
        {
            Console.WriteLine($"Member {memberId} joining group {matchId}");
            try
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, matchId);
                Console.WriteLine($"Member {memberId} successfully joined group {matchId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error joining group: {ex.Message}");
                throw;
            }
        }

        public async Task LeaveGroup(string matchId)
        {
            Console.WriteLine($"Leaving group {matchId}");
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, matchId);
        }

    }
}
