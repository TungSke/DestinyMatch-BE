using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPT.DestinyMatch.Service.Models.Response
{
    public class MemberResponse
    {
        public Guid Id { get; set; }

        public string? Fullname { get; set; }

        public string? Introduce { get; set; }

        public DateOnly? Dob { get; set; }

        public bool? Gender { get; set; }

        public string? Address { get; set; }

        public int? Surplus { get; set; }

        public string? Status { get; set; }

        public string? UniversityName { get; set; }

        public string? MajorName { get; set; }

        public ICollection<string> Hobbies { get; set; }

        public ICollection<string> UrlPath { get; set; }
    }
}
