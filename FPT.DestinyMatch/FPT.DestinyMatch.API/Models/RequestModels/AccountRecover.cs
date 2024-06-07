using System.ComponentModel.DataAnnotations;

namespace FPT.DestinyMatch.API.Models.RequestModels
{
    public class AccountRecover
    {
        [Required] [EmailAddress] public string Email { get; set; }
        public string Status { get; set; }//Member = "experienced"       //Staff = "working"
    }
}
