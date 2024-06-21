using System.ComponentModel.DataAnnotations;

namespace FPT.DestinyMatch.API.Models.RequestModels
{
    public class AccountNewPassword
    {
        public Guid Id { get; set; }

        public string? OldPassword { get; set; }

        [Required] public string NewPassword { get; set; }
    }
}
