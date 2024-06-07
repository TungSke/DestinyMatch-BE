using BusinessLogic.Interfaces;
using BusinessLogic.Models.Request;
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
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;

        public MemberService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public  async Task<Member> CreateMember(MemberRequest memberRequest)
        {
            DateTime? dob = memberRequest.Dob != null
        ? new DateTime(memberRequest.Dob.Year, memberRequest.Dob.Month, memberRequest.Dob.Day)
        : (DateTime?)null;
            var MemberToAdd = new Member
            {
                Id = Guid.NewGuid(),
                Fullname = memberRequest.Fullname,
                Introduce = memberRequest.Introduce,
                Dob = dob,
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
            if (member == null)
            {
                return false;
            }
            _memberRepository.Remove(member);
            await _memberRepository.SaveChangeAsync();
            return true;
        }

        public async Task<Member?> GetMemberById(Guid id)
        {
            return await _memberRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Member>> GetMembers()
        {
            return await _memberRepository.GetAllAsync().ToListAsync();
        }

        public async Task<Member> UpdateMember(Guid Id, MemberRequest memberRequest)
        {
            var member = await _memberRepository.GetByIdAsync(Id);
            if (member == null)
            {
                return null;
            }
            member.Fullname = !string.IsNullOrEmpty(memberRequest.Fullname) ? memberRequest.Fullname : member.Fullname;
            member.Introduce = !string.IsNullOrEmpty(memberRequest.Introduce) ? memberRequest.Introduce : member.Introduce;
            if(memberRequest.Dob != null)
            {
                member.Dob = new DateTime(memberRequest.Dob.Year, memberRequest.Dob.Month, memberRequest.Dob.Day);
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
