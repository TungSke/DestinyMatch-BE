using BusinessLogic.Interfaces;
using BusinessLogic.Models.Request;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
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
            if (message == null)
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
            return await _messageReposirory.GetAllAsync().ToListAsync();
        }

        public async Task<Message> UpdateMessage(Guid Id, MessageRequest messageRequest)
        {
            var message = await _messageReposirory.GetByIdAsync(Id);
            if (message == null)
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
    }
}
