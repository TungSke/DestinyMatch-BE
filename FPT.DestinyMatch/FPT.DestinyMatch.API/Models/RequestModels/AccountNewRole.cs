using System.ComponentModel.DataAnnotations;

namespace FPT.DestinyMatch.API.Models.RequestModels
{
    public class AccountNewRole
    {
        public Guid Id { get; set; }

        [AllowedValues("admin", "moderator", "member")] public string NewRole { get; set; }
    }
}
