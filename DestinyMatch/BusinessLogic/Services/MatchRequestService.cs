using BusinessLogic.Interfaces;
using BusinessLogic.Models.Request;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class MatchRequestService: IMatchRequestService
    {
        private readonly IMatchRequestRepository _matchRequestRepository;

        public MatchRequestService(IMatchRequestRepository matchRequestRepository)
        {
            _matchRequestRepository = matchRequestRepository;
        }

        public async Task<MatchRequest?> GetMatchRequestById(Guid matchRequestId)
        {
            var matchRequest = await _matchRequestRepository.GetByIdAsync(matchRequestId);
            if(matchRequest == null)
            {
                return null;
            }
            return matchRequest;
        }

        public async Task<IEnumerable<MatchRequest>?> GetMatchRequests()
        {
            return await _matchRequestRepository.GetAllAsync().ToListAsync();
        }

        public async Task<MatchRequest> Matching(MatchRequestToAdd matchRequesttoAdd)
        {
            var matchRequest = new MatchRequest
            {
                CreateAt = DateTime.Now,
                Status = "false",
                FromId = matchRequesttoAdd.FromId,
                ToId = matchRequesttoAdd.ToId,
            };
            _matchRequestRepository.Add(matchRequest);
            await _matchRequestRepository.SaveChangeAsync();
            return matchRequest;
        }

        public async Task<List<MatchRequest>> MatchRequestOfMe(Guid memberId)
        {
            return  await _matchRequestRepository.MatchRequestOfMe(memberId);
        }

        public async Task<List<MatchRequest>> MatchRequestToMe(Guid memberId)
        {
            return await _matchRequestRepository.MatchRequestToMe(memberId);
        }

        public async Task<MatchRequest> MatchResponse(Guid matchRequestId, string response)
        {
            var matchRequest = await _matchRequestRepository.GetByIdAsync(matchRequestId);
            if(matchRequest == null)
            {
                return null;
            }
            matchRequest.Status = response;
            await _matchRequestRepository.SaveChangeAsync();
            return matchRequest;
        }   

        public async Task<bool> RemoveMatchRequest(Guid matchRequestId)
        {
            var matchRequest = await _matchRequestRepository.GetByIdAsync(matchRequestId);
            if(matchRequest == null)
            {
                return false;
            }
            _matchRequestRepository.Remove(matchRequest);
            await _matchRequestRepository.SaveChangeAsync();
            return true;
        }
    }
}
