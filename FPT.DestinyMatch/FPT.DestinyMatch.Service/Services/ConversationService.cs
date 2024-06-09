using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;
using FPT.DestinyMatch.Service.Interfaces;
using Microsoft.IdentityModel.Tokens;
using FPT.DestinyMatch.Service.Extensions.Exceptions;

namespace FPT.DestinyMatch.Service.Services
{
    public class ConversationService : IConversationService
    {
        private readonly IConversationRepository _conversationRepository;
        public ConversationService(IConversationRepository conversationRepository)
        {
            _conversationRepository = conversationRepository;
        }

        //--------------------------[ IMPLEMENT ]--------------------------
        public async Task<Conversation> GetConversationDetail(Guid conversationId, Guid memberUsingId)
        {
            var currentConversation = await _conversationRepository.GetByIdAsync(conversationId);
            string currentStatus = currentConversation.Status.ToLower();

            // Validate Member having this conversation
            if(currentConversation.FirstMemberId != memberUsingId || currentConversation.SecondMemberId != memberUsingId)
            {
                throw new BadRequestException("You are not a member of this conversation");
            }

            // Validate available conversation
            if(currentStatus.Equals("deleted") || currentStatus.IsNullOrEmpty())
            {
                throw new NotFoundException("This conversation is deleted or not found");
            }
            return currentConversation;
        }

        public async Task<bool> ChangeNameConversation (Guid conversationId, string newName)
        {
            var currentConversation = await _conversationRepository.GetByIdAsync(conversationId);
            if (currentConversation is null || newName.IsNullOrEmpty())
            {
                return false;
            }
            currentConversation.Name = newName;
            return await _conversationRepository.SaveChangeAsync();
        }

        public async Task<bool> DeleteConversation(Guid conversationId)
        {
            var currentConversation = await _conversationRepository.GetByIdAsync(conversationId);
            if (currentConversation is null)
            {
                return false;
            }
            currentConversation.Status = "deleted";
            /*Delete Message Logic
             
             */
            return await _conversationRepository.SaveChangeAsync();
        }
    }
}
