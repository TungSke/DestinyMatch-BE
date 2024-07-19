using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.Service.Models.Request;
using Microsoft.EntityFrameworkCore;
using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using FPT.DestinyMatch.Service.Models.Response;
using FPT.DestinyMatch.Repository.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

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

        public async Task<MemberResponse?> GetMemberById(Guid id)
        {
            var member = await _memberRepository.GetMemberById(id);
            return member.Adapt<MemberResponse>();
        }

        public async Task<bool> CheckAccountExistsInMember(Guid accountId)
        {
            return await _memberRepository.CheckAccountExistsInMember(accountId);
        }

        public async Task<Member> GetMemberByAccountId(Guid id)
        {
            return await _memberRepository.GetAsync().FirstOrDefaultAsync(x => x.AccountId == id);
        }

        public async Task<(IEnumerable<MemberResponse> members, int totalCount)> GetMembers(string search, bool? gender, int? minAge, int? maxAge, int page, int pagesize)
        {
            var members = _memberRepository.GetAsync()
                .Include(m => m.Pictures)
                .Include(x => x.University)
                .Include(x => x.Major)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                members = members.Where(m => m.Fullname.ToLower().Contains(search) || m.Major.Name.ToLower().Contains(search));
            }

            if (minAge.HasValue && maxAge.HasValue)
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                var minDob = today.AddYears(-maxAge.Value);
                var maxDob = today.AddYears(-minAge.Value);

                members = members.Where(mem => mem.Dob >= minDob && mem.Dob <= maxDob);
            }

            if (gender.HasValue)
            {
                members = members.Where(m => m.Gender == gender.Value);
            }

            page = page == 0 ? 1 : page;
            pagesize = pagesize == 0 ? 5 : pagesize;

            var totalCount = await members.CountAsync();
            members = members.Skip((page - 1) * pagesize).Take(pagesize);

            var memberResponses = members.Adapt<IEnumerable<MemberResponse>>();

            return (memberResponses, totalCount);
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
            _memberRepository.Update(member);
            await _memberRepository.SaveChangeAsync();
            return member;
        }
    }
}
