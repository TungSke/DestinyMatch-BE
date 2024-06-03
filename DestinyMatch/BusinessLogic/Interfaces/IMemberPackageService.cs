using BusinessLogic.Models.Request;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IMemberPackageService
    {
        Task<IEnumerable<MemberPackage>> GetMemberPackages();

        Task<MemberPackage?> GetMemberPackageById(Guid id);
        Task<bool> DeleteMeberPackage(Guid Id);
        Task<MemberPackage> CreateMemberPackage(MemberPackageRequest memberRequest);
        Task<MemberPackage> UpdateMemberPackage(Guid Id, MemberPackageRequest memberRequest);
    }
}
