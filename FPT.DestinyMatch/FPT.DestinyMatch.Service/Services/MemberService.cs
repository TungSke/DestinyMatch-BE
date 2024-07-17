using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.Service.Models.Request;
using Microsoft.EntityFrameworkCore;
using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;
using FPT.DestinyMatch.Service.Extensions.Exceptions;

namespace FPT.DestinyMatch.Service.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;

        public MemberService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<Member> CreateMember(MemberRequest memberRequest)
        {
            //    DateTime? dob = memberRequest.Dob is not null
            //? new DateTime(memberRequest.Dob.Year, memberRequest.Dob.Month, memberRequest.Dob.Day)
            //: (DateTime?)null;
            var MemberToAdd = new Member
            {
                Id = Guid.NewGuid(),
                Fullname = memberRequest.Fullname,
                Introduce = memberRequest.Introduce,
                Dob = memberRequest.Dob,
                Gender = memberRequest.Gender,
                Address = memberRequest.Address,
                Surplus = memberRequest.Surplus,
                Status = memberRequest.Status,
                AccountId = memberRequest.AccountId,
                UniversityId = memberRequest.UniversityId,
                MajorId = memberRequest.MajorId
            };
            _memberRepository.Add(MemberToAdd);
            await _memberRepository.SaveChangeAsync();
            return MemberToAdd;
        }

        public async Task<bool> DeleteMeber(Guid memberId)
        {
            var member = await _memberRepository.GetByIdAsync(memberId);
            if (member is null)
            {
                return false;
            }
            _memberRepository.Remove(member);
            await _memberRepository.SaveChangeAsync();
            return true;
        }

        public async Task<Member?> GetMemberById(Guid id)
        {
            return await _memberRepository.GetMemberById(id);
        }

        public async Task<bool> CheckAccountExistsInMember(Guid accountId)
        {
            return await _memberRepository.CheckAccountExistsInMember(accountId);
        }

        public async Task<Member> GetMemberByAccountId(Guid id)
        {
            return await _memberRepository.GetAsync().FirstOrDefaultAsync(x => x.AccountId == id);
        }
        public async Task<Member> UpdateMember(Guid Id, MemberRequest memberRequest)
        {
            var member = await _memberRepository.GetByIdAsync(Id);
            if (member is null)
            {
                return null;
            }
            member.Fullname = !string.IsNullOrEmpty(memberRequest.Fullname) ? memberRequest.Fullname : member.Fullname;
            member.Introduce = !string.IsNullOrEmpty(memberRequest.Introduce) ? memberRequest.Introduce : member.Introduce;
            if (memberRequest.Dob is not null)
            {
                member.Dob = memberRequest.Dob;//new DateTime(memberRequest.Dob.Year, memberRequest.Dob.Month, memberRequest.Dob.Day);
            }
            member.Gender = memberRequest.Gender ?? member.Gender;
            member.Address = !string.IsNullOrEmpty(memberRequest.Address) ? memberRequest.Address : member.Address;
            member.Surplus = memberRequest.Surplus ?? member.Surplus;
            member.Status = !string.IsNullOrEmpty(memberRequest.Status) ? memberRequest.Status : member.Status;
            member.AccountId = memberRequest.AccountId;
            member.UniversityId = memberRequest.UniversityId;
            member.MajorId = memberRequest.MajorId;
            await _memberRepository.Update(member);
            await _memberRepository.SaveChangeAsync();
            return member;
        }
        public async Task<(IEnumerable<Member> ResultList, int TotalCount, int CurrentPage, int CurrentAmount)>
            SearchMember(int amount, int pageIndex, string? emailKeyword, string? nameKeyword, bool? genderType, string? statusType, string? universityKeyword, string? majorKeyword, List<string>? hobbyList, int? minAge, int? maxAge, bool orderByName_Descending)
        {
            var tuppleObj = await _memberRepository.GetListMember_Search(amount, pageIndex, emailKeyword, nameKeyword, genderType, statusType, universityKeyword, majorKeyword, hobbyList, minAge, maxAge, orderByName_Descending);
            if (tuppleObj.ResultList is null || !tuppleObj.ResultList.Any())
            {
                throw new NotFoundException("Not found any Member suitable that filter");
            }
            return (tuppleObj.ResultList, tuppleObj.TotalCount, tuppleObj.CurrentPage, tuppleObj.CurrentAmount);
        }
    }
}
