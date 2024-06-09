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
        public async Task<Conversation?> GetConversationDetail(Guid conversationId, Guid memberUsingId)
        {
            var currentConversation = await _conversationRepository.GetByIdAsync(conversationId);
            string currentStatus = currentConversation.Status;

            // Validate Member having in this conversation
            ValidateMemberInConversation(currentConversation, memberUsingId);

            // Validate available conversation
            ValidateConversationIsDeleted(currentStatus);
            return currentConversation;
        }
        public async Task<Conversation> StartNewConversation(Guid fromMemberId, Guid toMemberId)
        {
            var trySearch = await _conversationRepository.GetByFilterAsync(cv => cv.FirstMemberId == fromMemberId && cv.SecondMemberId == toMemberId);
            var existedConversation = trySearch is null ?//if null -> search in reverse. If not -> assign
                await _conversationRepository.GetByFilterAsync
                (cv => cv.FirstMemberId == toMemberId && cv.SecondMemberId == fromMemberId) : trySearch;

            if (existedConversation is not null && !existedConversation.Status.ToLower().Equals("deleted"))
            {
                return existedConversation;
            }

            var newConversation = await _conversationRepository.Add(new Conversation
            {
                FirstName = "",
                FirstMemberId = fromMemberId,
                SecondMemberId = toMemberId,
                Status = "Created"
            });
            await _conversationRepository.SaveChangeAsync();
            return newConversation;
        }
        public async Task<bool> ChangeNameConversation(Guid conversationId, Guid interactingMemberId, string newName)
        {
            var currentConversation = await _conversationRepository.GetByIdAsync(conversationId);
            if (currentConversation is null || newName.IsNullOrEmpty())
            {
                throw new BadRequestException("Not found this conversation or cannot have null name");
            }
            ValidateConversationIsDeleted(currentConversation.Status);
            ValidateMemberInConversation(currentConversation, interactingMemberId);

            if(currentConversation.FirstMemberId == interactingMemberId)
            {
                currentConversation.SecondName = newName;
                return await _conversationRepository.SaveChangeAsync();
            }
            currentConversation.FirstName = newName;
            return await _conversationRepository.SaveChangeAsync();
        }

        public async Task<bool> DeleteConversation(Guid conversationId)
        {
            var currentConversation = await _conversationRepository.GetByIdAsync(conversationId);
            if (currentConversation is null)
            {
                throw new BadRequestException("Not found this conversation Id");
            }
            // Validate if deleted
            ValidateConversationIsDeleted(currentConversation.Status);

            currentConversation.Status = "deleted";
            return await _conversationRepository.SaveChangeAsync();
        }

        private static void ValidateMemberInConversation(Conversation conversation, Guid memberId)
        {
            if (conversation.FirstMemberId != memberId && conversation.SecondMemberId != memberId)
            {
                throw new BadRequestException("You are not a member of this conversation");
            }
        }
        private static void ValidateConversationIsDeleted(string conversationStatus)
        {
            if (conversationStatus.ToLower().Equals("deleted") || conversationStatus.IsNullOrEmpty())
            {
                throw new NotFoundException("This conversation is deleted or not available");
            }
        }
    }
}
