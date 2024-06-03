using BusinessLogic.Models.Request;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<Member>> GetMembers();
        
        Task<Member?> GetMemberById(Guid id);
        Task<bool> DeleteMeber(Guid memberId);
         Task<Member> CreateMember(MemberRequest memberRequest);
         Task<Member> UpdateMember(Guid Id, MemberRequest memberRequest);
        
    }
}
