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
        private readonly IMemberRepository _memberRepository;
        public ConversationService(IConversationRepository conversationRepository, IMemberRepository memberRepository)
        {
            _conversationRepository = conversationRepository;
            _memberRepository = memberRepository;
        }

        //--------------------------[ IMPLEMENT ]--------------------------
        public async Task<Conversation> GetConversationDetailAsync(Guid conversationId, Guid memberUsingId)
        {
            var currentConversation = await _conversationRepository.GetByIdAsync(conversationId);
            if (currentConversation is null)
            {
                throw new NotFoundException("Not found this Conversation id");
            }

            // Validate Member having in this conversation
            ValidateMemberInConversation(currentConversation, memberUsingId);

            // Validate available conversation
            ValidateConversationIsDeleted(currentConversation.Status);
            return currentConversation;
        }
        public async Task<IEnumerable<Conversation>> GetRecentlyConversationListAsync(Guid memberId, int pageIndex)
        {
            var partialList = await _conversationRepository.GetRecentlyListAsync(memberId, pageIndex);
            if (!partialList.Any())
            {
                throw new NotFoundException("Not found any conversation");
            }
            return partialList;
        }
        public async Task<IEnumerable<Conversation>> SearchConversationsListAsync(int size, int page, Guid memberUsingId,
            string? keyword, string? status, bool isDescending)
        {
            var partialList = await _conversationRepository.GetFilteredListAsync(size, page, memberUsingId, keyword, status, isDescending);
            if (!partialList.Any())
            {
                throw new NotFoundException("Not found any conversation");
            }
            return partialList;
        }

        public async Task<bool> StartNewConversationAsync(Guid fromMemberId, Guid toMemberId)
        {
            var trySearch = await _conversationRepository
                .GetByFilterAsync(cv1 => cv1.FirstMemberId == fromMemberId && cv1.SecondMemberId == toMemberId);

            var existedConversation = trySearch is not null ? trySearch : //if not null -> Assign. Else search in reverse
                await _conversationRepository
                .GetByFilterAsync(cv2 => cv2.SecondMemberId == fromMemberId && cv2.FirstMemberId == toMemberId);

            if (existedConversation is not null && !existedConversation.Status!.ToLower().Equals("deleted"))
            {
                throw new ConflictException("Cannot start new Conversation due to existed a conversation with same member");
            }

            var first_member = await _memberRepository.GetByIdAsync(fromMemberId) ?? throw new NotFoundException("From Member Id is not exist");
            var second_member = await _memberRepository.GetByIdAsync(toMemberId) ?? throw new NotFoundException("To Member Id is not exist");

            var newConversation = await _conversationRepository.Add(new Conversation
            {
                FirstName = first_member.Fullname,
                SecondName = second_member.Fullname,
                FirstMemberId = fromMemberId,
                SecondMemberId = toMemberId,
                Status = "Created"
            });
            return await _conversationRepository.SaveChangeAsync();
        }
        public async Task<bool> ChangeNameConversationAsync(Guid conversationId, Guid interactingMemberId, string newName)
        {
            var currentConversation = await _conversationRepository.GetByIdAsync(conversationId);
            if (currentConversation is null || newName.IsNullOrEmpty())
            {
                throw new BadRequestException("Not found this conversation or cannot have null name");
            }
            ValidateConversationIsDeleted(currentConversation.Status);
            ValidateMemberInConversation(currentConversation, interactingMemberId);

            if (interactingMemberId == currentConversation.FirstMemberId)
            {
                currentConversation.SecondName = newName;
                return await _conversationRepository.SaveChangeAsync();
            }
            currentConversation.FirstName = newName;
            return await _conversationRepository.SaveChangeAsync();
        }

        public async Task<bool> DeleteConversationAsync(Guid conversationId)
        {
            var currentConversation = await _conversationRepository.GetByIdAsync(conversationId)?? throw new NotFoundException("Not found this conversation Id");

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
