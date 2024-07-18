using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;
using FPT.DestinyMatch.Service.Extensions.Exceptions;
using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.Service.Models.Request;
using FPT.DestinyMatch.Service.Models.Response;
using Mapster;

namespace FPT.DestinyMatch.Service.Services
{
    public class MatchingService : IMatchingService
    {
        private readonly IMatchingRepository _matchingRepository;
        private readonly IMemberRepository _membberRepo;
        public MatchingService(IMatchingRepository matchingRepository, IMemberRepository membberRepo)
        {
            _matchingRepository = matchingRepository;
            _membberRepo=membberRepo;
        }

        public async Task<IEnumerable<MatchingResponse>> GetMatchings(Guid currentMemId, int pageIndex, int pageSize, string search, string status)
        {
            var list = await _matchingRepository.GetMatchings(currentMemId, pageIndex, pageSize, search, status);
            var matchings = list
        .OrderBy(m => m.CreatedAt)
        .Select(m => new MatchingResponse
        {
            ConversationId = m.Id,
            ParticipantFullName = m.FirstMemberId == currentMemId
                ? m.SecondMember?.Fullname ?? "Unknown"
                : m.FirstMember?.Fullname ?? "Unknown",
            ParticipantAvatarUrl = m.FirstMemberId == currentMemId
                ? m.SecondMember?.Pictures.FirstOrDefault(x => x.IsAvatar == true)?.UrlPath ?? "https://firebasestorage.googleapis.com/v0/b/destinymatch-70b72.appspot.com/o/imgs%2Fdefault-avatar-profile-icon-of-social-media-user-vector.jpg?alt=media&token=6ebdc7fd-6433-4c0d-9c36-d80fd848ce41"
                : m.FirstMember?.Pictures.FirstOrDefault(x => x.IsAvatar == true)?.UrlPath ?? "https://firebasestorage.googleapis.com/v0/b/destinymatch-70b72.appspot.com/o/imgs%2Fdefault-avatar-profile-icon-of-social-media-user-vector.jpg?alt=media&token=6ebdc7fd-6433-4c0d-9c36-d80fd848ce41",
            LastMessage = m.Messages?.OrderByDescending(msg => msg.SentAt).FirstOrDefault()?.Content ?? "No messages",
            LastMessageTime = m.Messages?.OrderByDescending(msg => msg.SentAt).FirstOrDefault()?.SentAt
        })
        .ToList();
            return matchings;
        }

        public async Task CreateMatching(MatchingRequest matchingrequest)
        {
            var matching = matchingrequest.Adapt<Matching>();
            var firstMember = await _membberRepo.GetByIdAsync(matchingrequest.thisMemberId);
            var secondMember = await _membberRepo.GetByIdAsync(matchingrequest.toMemberId);
            matching.Status = "success";
            matching.FirstMemberId = firstMember.Id;
            matching.SecondMemberId = secondMember.Id;
            matching.FirstName = firstMember.Fullname;
            matching.SecondName = secondMember.Fullname;
            matching.CreatedAt = DateTime.Now;
            await _matchingRepository.Add(matching);
            await _matchingRepository.SaveChangeAsync();
        }


        public async Task DeleteMatching(Guid matchid)
        {
            var match = await _matchingRepository.GetByIdAsync(matchid);
            if (match == null)
            {
                throw new NotFoundException("not found this conversation");
            }
            await _matchingRepository.Remove(match);
            await _matchingRepository.SaveChangeAsync();
        }

        public async Task ChangeStatusMatching(Guid matchid, string status)
        {
            var match = await _matchingRepository.GetByIdAsync(matchid);
            if (match == null)
            {
                throw new NotFoundException("not found this conversation");
            }
            match.Status = status;
            await _matchingRepository.SaveChangeAsync();
        }
    }
}
