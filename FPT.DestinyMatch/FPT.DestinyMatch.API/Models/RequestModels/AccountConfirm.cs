using System.ComponentModel.DataAnnotations;

namespace FPT.DestinyMatch.API.Models.RequestModels
{
    public class AccountConfirm
    {
        public Guid Id { get; set; }
        [Required] public required string Password { get; set; }
    }
}
