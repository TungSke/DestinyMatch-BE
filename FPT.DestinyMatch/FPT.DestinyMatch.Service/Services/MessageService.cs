using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;
using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.Service.Models.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FPT.DestinyMatch.Service.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageReposirory _messageReposirory;
        private readonly IFirebaseService _firebaseService;
        private readonly IMatchingRepository _matchingRepository;
        public MessageService(IMessageReposirory messageReposirory, IFirebaseService firebaseService, IMatchingRepository matchingRepository)
        {
            _messageReposirory = messageReposirory;
            _firebaseService = firebaseService;
            _matchingRepository=matchingRepository;
        }
        public async Task<Message> CreateMessage(MessageRequest messageRequest)
        {
            var messageToAdd = new Message
            {
                Id = Guid.NewGuid(),
                Content = messageRequest.Content,
                SentAt = DateTime.UtcNow,
                Status = messageRequest.Status,
                MatchingId = messageRequest.MatchId,
                SenderId = messageRequest.SenderId
            };
            _messageReposirory.Add(messageToAdd);
            await _messageReposirory.SaveChangeAsync();

            var FcmTokenopponent = await _matchingRepository.GetAsync()
                .Where(m => m.Id == messageRequest.MatchId)
            .Include(m => m.FirstMember)
                .ThenInclude(m => m.Account)
            .Include(m => m.SecondMember)
                .ThenInclude(m => m.Account)
            .Select(m => m.FirstMemberId != messageRequest.SenderId ? m.FirstMember.Account.FcmtToken : m.SecondMember.Account.FcmtToken)
            .FirstOrDefaultAsync();
            
            if (!FcmTokenopponent.IsNullOrEmpty())
            {
                await _firebaseService.SendNotification(FcmTokenopponent, "Bạn vừa nhận được tin nhắn mới", messageRequest.Content);
            }
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
            message.MatchingId = messageRequest.MatchId;
            message.SenderId = messageRequest.SenderId;
            _messageReposirory.Update(message);
            await _messageReposirory.SaveChangeAsync();
            return message;
        }

        public async Task<IEnumerable<Message>> GetMessagesByConversationId(Guid MatchId)
        {
            return await _messageReposirory.GetAsync().Where(m => m.MatchingId == MatchId).OrderBy(x => x.SentAt).ToListAsync();
        }
    }
}
