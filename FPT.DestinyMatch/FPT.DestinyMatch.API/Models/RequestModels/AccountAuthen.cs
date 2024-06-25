using System.ComponentModel.DataAnnotations;

namespace FPT.DestinyMatch.API.Models.RequestModels
{
    public class AccountAuthen
    {
        [Required] [EmailAddress] public string Email { get; set; }

        [Required] public string Password { get; set; }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
