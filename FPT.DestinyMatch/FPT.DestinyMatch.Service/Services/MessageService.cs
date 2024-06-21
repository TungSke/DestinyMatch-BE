using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;
using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.Service.Models.Request;
using Microsoft.EntityFrameworkCore;

namespace FPT.DestinyMatch.Service.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageReposirory _messageReposirory;

        public MessageService(IMessageReposirory messageReposirory)
        {
            _messageReposirory = messageReposirory;
        }
        public async Task<Message> CreateMessage(MessageRequest messageRequest)
        {
            var messageToAdd = new Message
            {
                Id = Guid.NewGuid(),
                Content = messageRequest.Content,
                SentAt = DateTime.UtcNow,
                Status = messageRequest.Status,
                ConversationId = messageRequest.ConversationId,
                SenderId = messageRequest.SenderId
            };
            _messageReposirory.Add(messageToAdd);
            await _messageReposirory.SaveChangeAsync();
            return messageToAdd;
        }

        public async Task<bool> DeleteMessage(Guid memberId)
        {
            var message = await _messageReposirory.GetByIdAsync(memberId);
            if (message is null)
            {
                return false;
            }
            _messageReposirory.Remove(message);
            await _messageReposirory.SaveChangeAsync();
            return true;
        }

        public async Task<Message?> GetMessageById(Guid id)
        {
            return await _messageReposirory.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Message>> GetMessages()
        {
            return await _messageReposirory.GetAsync().ToListAsync();
        }

        public async Task<Message> UpdateMessage(Guid Id, MessageRequest messageRequest)
        {
            var message = await _messageReposirory.GetByIdAsync(Id);
            if (message is null)
            {
                return null;
            }
            message.Content = !string.IsNullOrEmpty(messageRequest.Content) ? messageRequest.Content : message.Content;
            message.SentAt = DateTime.UtcNow;
            message.Status = !string.IsNullOrEmpty(messageRequest.Status) ? messageRequest.Status : message.Status;
            message.ConversationId = messageRequest.ConversationId;
            message.SenderId = messageRequest.SenderId;
            _messageReposirory.Update(message);
            await _messageReposirory.SaveChangeAsync();
            return message;
        }

        public async Task<IEnumerable<Message>> GetMessagesByConversationId(Guid conversationId)
        {
            return await _messageReposirory.GetAsync().Where(m => m.ConversationId == conversationId).ToListAsync();
        }
    }
}
